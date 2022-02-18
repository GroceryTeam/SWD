using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
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

namespace SWD_GSM_SystemAdmin.Controllers.SystemAdmin
{
    [Route(SystemAdminRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]

    public class StoresController : BaseSystemAdminController
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] StoreSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            try
            {
                //check storeId
                paging = PagingUtil.checkDefaultPaging(paging);
                var stores = await _storeService.GetStoreList( searchModel, paging);
                return Ok(stores);
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
                var product = await _storeService.GetStoreById(id);
                if (product != null)
                {
                    return Ok(product);
                }
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

