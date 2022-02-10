using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SWD_GSM.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    [ApiExplorerSettings(GroupName = Role)]
    public class LoginController : BaseStoreOwnerController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
                if (login == null)
                {
                    return BadRequest();
                }
                var user = await _userService.Login(login);
                if (user != null)
                {
                    string tokenString = CreateAuthenToken.GetToken(Role);
                    return Ok(new BaseLoginViewModel<StoreOwnerViewModel>()
                    {
                        Token = tokenString,
                        Information = user
                    });
                }
                else
                {
                    return Unauthorized();
                }
        }
    }
}
