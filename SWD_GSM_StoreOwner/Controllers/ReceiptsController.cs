using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
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

namespace SWD_GSM_StoreOwner.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    //[Authorize(Roles = Role)]
    public class ReceiptsController : BaseStoreOwnerController
    {
        private readonly BusinessLayer.Interfaces.StoreOwner.IReceiptService _receiptService;

        public ReceiptsController(BusinessLayer.Interfaces.StoreOwner.IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int StoreId, [FromQuery] ReceiptSearchModel model, [FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var receipts = await _receiptService.GetReceiptList(StoreId, model, paging);
                return Ok(receipts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int StoreId, int id)
        {
            try
            {
                var paging = PagingUtil.getDefaultPaging();
                var receipt = await _receiptService.GetReceiptById(StoreId, id);
                return Ok(receipt);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewReceipt([FromBody] ReceiptCreateModel model)
        {
            try
            {
                if (model.StoreId <= 0
                    || model.TotalCost < 0)
                {
                    return BadRequest();
                }
                await _receiptService.AddReceipt(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }

}


