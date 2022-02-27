using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.Services;
using BusinessLayer.Services.Cashier;
using BusinessLayer.Services.StoreOwner;
using BusinessLayer.Services.SystemAdmin;
using BusinessLayer.Utilities;
using DataAcessLayer;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using BusinessLayer.Services.Notification;
using BusinessLayer.Interfaces.Notification;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace SWD_GSM_SystemAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });
            services.AddDbContext<GroceryCloud18th2Context>(
               options => options.UseSqlServer(Configuration.GetConnectionString("GroceryCloud")));

            services.AddControllers();
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            });

            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "System Admin API - Grocery Cloud", Version = "v1" });
                options.DocumentFilter<KebabCaseDocumentFilter>();

                options.TagActionsBy(api =>
                {
                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    string controllerName = controllerActionDescriptor.ControllerName;

                    if (api.GroupName != null)
                    {
                        var name = api.GroupName + controllerName.Replace("Controller", "");
                        name = Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
                        return new[] { name };
                    }

                    if (controllerActionDescriptor != null)
                    {
                        controllerName = Regex.Replace(controllerName, "([a-z])([A-Z])", "$1 $2");
                        return new[] { controllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                                { jwtSecurityScheme, Array.Empty<string>() }
                            });

                options.DocInclusionPredicate((name, api) => true);
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,

                  ValidIssuer = "http://localhost:2000",
                  ValidAudience = "http://localhost:2000",
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"))
              };
          });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //admin
            services.AddTransient<BusinessLayer.Interfaces.SystemAdmin.IBrandService,
                 BusinessLayer.Services.SystemAdmin.BrandService>();
            services.AddTransient<BusinessLayer.Interfaces.SystemAdmin.IStoreService,
                BusinessLayer.Services.SystemAdmin.StoreService>();
            services.AddTransient<BusinessLayer.Interfaces.SystemAdmin.IUserService, BusinessLayer.Services.SystemAdmin.UserService>();
            IMapper mapper = AutoMapperConfig.config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<INotificationService,NotificationService>();
            services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Configuration["Firebase:Admin"]),
            })
          );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SWD_GSM v1"));

            app.UseRewriter(new RewriteOptions().Add(new PascalRule()));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
