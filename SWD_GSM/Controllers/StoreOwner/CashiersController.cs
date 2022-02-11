using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    [ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]
    public class CashiersController : BaseStoreOwnerController
    {
        private readonly ICashierService _cashierService;

        public CashiersController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CashierSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }
            try
            {
                //check storeId
                paging = PagingUtil.checkDefaultPaging(paging);
                var cashiers = await _cashierService.GetCashierList(searchModel, paging);
                return Ok(cashiers);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int BrandId, int id)
        //{
        //    return null;
        //}
       
    }
}

