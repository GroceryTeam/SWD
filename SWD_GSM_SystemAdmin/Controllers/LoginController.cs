using BusinessLayer.RequestModels.CreateModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using Utilities;

namespace SWD_GSM_SystemAdmin.Controllers.SystemAdmin
{
    [Route(SystemAdminRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    public class LoginController : BaseSystemAdminController
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            if (login.Username==config["AdminAccount:Username"] && 
                login.Password == config["AdminAccount:Password"])
            {
                string tokenString = CreateAuthenToken.GetToken(Role);
                return Ok(new {Token = tokenString});
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
