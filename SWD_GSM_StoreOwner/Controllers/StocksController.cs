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
    [Authorize(Roles = Role)]
    public class StocksController : BaseStoreOwnerController
    {
        private readonly IStoreService _storeService;

        public StocksController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int BrandId, [FromQuery] ProductSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            return null;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int BrandId, int id)
        {
            return null;
        }
        //Nhớ đặt tên lại bla bla, tào lao hết nhé
        [HttpPost]
        public async Task<IActionResult> CreateNewProduct(int BrandId, [FromBody] ProductCreateModel model)
        {
            //try
            //{
            //    if (model.ConversionRate <= 0
            //        || model.BuyPrice < 0
            //        || model.SellPrice < 0
            //        || model.LowerThreshold < 0)
            //    {
            //        return BadRequest();
            //    }
            //    var id = await _productService.AddProduct(BrandId, model);
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
            //return Ok();
            return null;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int BrandId, int id, [FromBody] ProductCreateModel model)
        {
            //try
            //{
            //    if (model.ConversionRate <= 0 || model.BuyPrice < 0 || model.SellPrice < 0 || model.LowerThreshold < 0)
            //    {
            //        return BadRequest();
            //    }
            //    await _productService.UpdateProduct(BrandId, id, model);
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
            //return Ok();
            return null;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int BrandId, int id)
        {
            //try
            //{
            //    var result = await _productService.DeleteProduct(BrandId, id);
            //    if (result.InverseUnpackedProducts.Count == 0)
            //    {
            //        return NoContent();
            //    }
            //    else
            //    {
            //        return Conflict(result);
            //    }
            //}
            //catch (Exception)
            //{
            //    return NotFound();
            //}
            return null;
        }
    }
}

