﻿using BusinessLayer.Constants;
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

    public class EventsController : BaseStoreOwnerController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int BrandId, int id)
        {
            try
            {
                var paging = PagingUtil.getDefaultPaging();
                var _event = await _eventService.GetEventById(BrandId, id);
                return Ok(_event);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int BrandId, [FromQuery] EventSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var products = await _eventService.GetEventList(BrandId, searchModel, paging);
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

