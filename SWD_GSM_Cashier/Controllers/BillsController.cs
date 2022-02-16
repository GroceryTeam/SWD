using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.Cashier;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_Cashier.Controllers.Cashier
{
    [Route(CashierRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]
    public class BillsController : BaseCashierController
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int StoreId, int CashierId, [FromBody] BillCreateModel model)
        {
            try
            {
                await _billService.AddBill(StoreId, CashierId, model);
            } catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}

