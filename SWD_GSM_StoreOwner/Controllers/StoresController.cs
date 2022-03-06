using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
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

    public class StoresController : BaseStoreOwnerController
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int BrandId)
        {
            try
            {
                var stores = await _storeService.GetStoreList(BrandId);
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
        [HttpPost]
        public async Task<IActionResult> CreateNewStore([FromBody] StoreCreateModel model)
        {
            try
            {
                if (model.Name.Length == 0
                    || model.Address.Length == 0
                    || model.BrandId <= 0)
                {
                    return BadRequest();
                }
               await _storeService.CreateStore(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StoreCreateModel model)
        {
            //check store thuộc brnad
            try
            {
                if (model.Name.Length == 0
                    || model.Address.Length == 0
                    || model.BrandId <= 0)
                {
                    return BadRequest();
                }
                await _storeService.UpdateStore(id, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //check store co thuoc brand ko
            try
            {
                await _storeService.DeleteStore(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}

