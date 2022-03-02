using BusinessLayer.Interfaces.Notification;
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

namespace SWD_GSM_StoreOwner.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    public class LoginController : BaseStoreOwnerController
    {
        private readonly IUserService _userService;
        private readonly IFCMTokenService _fcmService;
        public LoginController(IUserService userService, IFCMTokenService fcmService)
        {
            _userService = userService;
            _fcmService = fcmService;
        }
        [HttpPost("token")]
        public IActionResult RegisterDevice([FromBody] FcmTokenModel model)
        {
            try
            {
                _fcmService.AddToken(model.TokenId, model.UserId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("token")]
        public async Task<IActionResult> UnregisterDevice([FromBody] FcmTokenModel model)
        {
            try
            {
                await _fcmService.DeleteToken(model.TokenId, model.UserId);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
                    return Ok(new BaseLoginViewModel<UserViewModel>()
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
        [HttpPost("firebase")]
        public async Task<IActionResult> Login([FromBody] LoginFirebaseModel login)
        {
            if (login == null)
            {
                return BadRequest();
            }
            var user = await _userService.LoginFirebase(login);
            if (user != null)
            {
                string tokenString = CreateAuthenToken.GetToken(Role);
                return Ok(new BaseLoginViewModel<UserViewModel>()
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
