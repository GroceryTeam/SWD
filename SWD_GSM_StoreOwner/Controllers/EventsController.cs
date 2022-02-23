using BusinessLayer.Constants;
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
using static DataAcessLayer.Models.Event;

namespace SWD_GSM_StoreOwner.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
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
                var events = await _eventService.GetEventList(BrandId, searchModel, paging);
                return Ok(events);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewEvent([FromBody] EventCreateModel model)
        {
            try
            {
                if (model.Details.Count <= 0)
                {
                    return BadRequest();
                }
                var id = await _eventService.AddEvent(model.BrandId, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EventCreateModel model)
        {
            try
            {
                await _eventService.UpdateEvent(id, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] EventCreateModel model)
        {
            try
            {
                if (model.Status == (int)EventStatus.Enabled)
                {
                    await _eventService.ApplyEvent(model.BrandId, id);
                }
                else
                {
                    await _eventService.UnApplyEvent(model.BrandId, id);
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            return Ok();

        }
        //[HttpPut("unapply-event/{id}")]
        //public async Task<IActionResult> UnapplyEvent(int BrandId, int id)
        //{
        //    try
        //    {
        //        await _eventService.UnApplyEvent(BrandId, id);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //}
        //get appliedEvent
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int BrandId, int id)
        {
            try
            {
                //var result = await _eventService.DeleteEvent(BrandId, id);
                //if (result.InverseUnpackedProducts.Count == 0)
                //{
                //    return NoContent();
                //}
                //else
                //{
                //    return Conflict(result);
                //}
                return Ok();

            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}

