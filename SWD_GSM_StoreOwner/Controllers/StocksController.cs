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
    public class StocksController : BaseStoreOwnerController
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int BrandId, [FromQuery] ProductStockSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            try
            {
                //check storeId
                paging = PagingUtil.checkDefaultPaging(paging);
                var productStocks = await _stockService.GetProductIncludingStock(searchModel, paging);
                return Ok(productStocks);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var stock = await _stockService.GetStockById(id);
                if (stock != null)
                {
                    return Ok(stock);
                }
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StockEditModel model)
        {
            try
            {
                if (id <= 0 )
                {
                    return BadRequest();
                }
                await _stockService.UpdateStock(id, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}

