using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD_GSM.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM.Controllers.SystemAdmin
{
    [Route(SystemAdminRoute)]
    [ApiController]
    [ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]


    public class UserController : BaseSystemAdminController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] StoreOwnerCreateModel model)
        {
            try
            {
                if (model.Username.Trim().Length == 0 
                    || model.Password.Trim().Length == 0
                    || model.Phone.Trim().Length == 0
                    || model.Email.Trim().Length == 0)
                {
                    return BadRequest();
                }
                var error = await _userService.Signup(model);
                if (error!=null)
                {
                    return Conflict(error);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int BrandId, int id)
        {
            return null;
        }
    }
}

