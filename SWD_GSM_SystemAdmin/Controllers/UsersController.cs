using BusinessLayer.Constants;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.Notification;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.Services.Notification;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_SystemAdmin.Controllers.SystemAdmin
{
    [Route(SystemAdminRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]
    public class UsersController : BaseSystemAdminController
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notiService;
        public UsersController(IUserService userService, INotificationService notiService)
        {
            _userService = userService;
            _notiService = notiService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paging = PagingUtil.getDefaultPaging();
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest();
            }
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DisableUser(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPut("{id}/enable")]
        public async Task<IActionResult> EnableUser(int id)
        {
            try
            {
                await _userService.EnableUser(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        //[HttpPost("ohnooooo")]
        //public IActionResult MesssageSend(int id)
        //{
        //    try
        //    {
        //        _notiService.SendNotificationOutOfStockProduct(1, 1, "Aloooo");
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(e.Message);
        //    }
        //    return Ok();
        //}
    }
}

