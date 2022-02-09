using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
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

namespace SWD_GSM.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    [ApiExplorerSettings(GroupName = Role)]
    [Authorize]
    [Authorize(Roles = "StoreOwner")]
    public class ReceiptController : BaseStoreOwnerController
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }
    
        [HttpGet]
        public async Task<IActionResult> Get(int StoreId, [FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = checkDefaultPaging(paging);
                var receipts = await _receiptService.GetReceiptList(StoreId, paging);
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
                var paging = getDefaultPaging();
                var receipt = await _receiptService.GetReceiptById(StoreId, id);
                return Ok(receipt);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
       

        [NonAction]
        private PagingRequestModel getDefaultPaging()
        {
            return new PagingRequestModel
            {
                PageIndex = PageConstant.DefaultPageIndex,
                PageSize = PageConstant.DefaultPageSize
            };
        }
        [NonAction]
        private PagingRequestModel checkDefaultPaging(PagingRequestModel paging)
        {
            if (paging.PageIndex <= 0) paging.PageIndex = PageConstant.DefaultPageIndex;
            if (paging.PageSize <= 0) paging.PageSize = PageConstant.DefaultPageSize;
            return paging;
        }
    }

}


