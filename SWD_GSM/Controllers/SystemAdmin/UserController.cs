using BusinessLayer.Constants;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.SystemAdmin;
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] UserSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var users = await _userService.GetUserList(searchModel, paging);
                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
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


        //[NonAction]
        //private PagingRequestModel getDefaultPaging()
        //{
        //    return new PagingRequestModel
        //    {
        //        PageIndex = PageConstant.DefaultPageIndex,
        //        PageSize = PageConstant.DefaultPageSize
        //    };
        //}
        //[NonAction]
        //private PagingRequestModel checkDefaultPaging(PagingRequestModel paging)
        //{
        //    if (paging.PageIndex <= 0) paging.PageIndex = PageConstant.DefaultPageIndex;
        //    if (paging.PageSize <= 0) paging.PageSize = PageConstant.DefaultPageSize;
        //    return paging;
        //}
    }
}

