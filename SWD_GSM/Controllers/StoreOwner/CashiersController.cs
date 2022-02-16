using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int BrandId, int id)
        {
            return null;
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewCashier([FromBody] CashierCreateModel model)
        {
            try
            {
                if (model.StoreId <= 0
                    || model.Username.Length == 0
                    || model.Password.Length == 0
                    || model.Name.Length == 0)
                {
                    return BadRequest();
                }
                await _cashierService.AddCashier(model.StoreId, model);
            }
            catch (DbUpdateException)
            {
                return Conflict(new CashierErrorModel
                {
                    Error = CashierErrorModel.CashierCreateError.UsernameDuplicated
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int CashierId,[FromBody] CashierCreateModel model)
        {
            try
            {
                if (model.StoreId <= 0
                     || model.Username.Length == 0
                     || model.Password.Length == 0
                     || model.Name.Length == 0)
                {
                    return BadRequest();
                }
                await _cashierService.UpdateCashier(CashierId, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int CashierId)
        {
            try
            {
                await _cashierService.DeleteCashier(CashierId);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
            //return null;
        }

    }
}

