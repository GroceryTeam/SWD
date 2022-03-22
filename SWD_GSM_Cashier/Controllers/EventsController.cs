using BusinessLayer.Constants;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD_GSM_Cashier.Controllers.Cashier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Event;

namespace SWD_GSM_Cashier.Controllers.StoreOwner
{
    [Route(CashierRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]

    public class EventsController : BaseCashierController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }


        [HttpGet("current-event")]
        public async Task<IActionResult> GetCurrentEvent(int BrandId)
        {
            try
            {
                var paging = PagingUtil.getDefaultPaging();
                var _event = await _eventService.GetCurrentAppliedEvent(BrandId);
                return Ok(_event);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

