using BusinessLayer.Constants;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_StoreOwner.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    public class UsersController : BaseStoreOwnerController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
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
                if (error != null)
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
        [HttpGet]
        public async Task<IActionResult> GetBySearchTerm([FromQuery] string SearchTerm)
        {
            try
            {
                var paging = PagingUtil.getDefaultPaging();
                var users = await _userService.GetUserByPhoneNoOrEmailOrUsername(SearchTerm);
                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

