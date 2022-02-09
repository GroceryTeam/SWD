using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
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
    public class ReceiptsController : BaseStoreOwnerController
    {
        private readonly BusinessLayer.Interfaces.StoreOwner.IReceiptService _receiptService;

        public ReceiptsController(BusinessLayer.Interfaces.StoreOwner.IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }
        [NonAction]
    

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return null;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int BrandId, int id)
        {
            return null;
        }
       
    }
}

