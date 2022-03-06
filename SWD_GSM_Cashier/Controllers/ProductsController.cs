using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels.Cashier;
using BusinessLayer.RequestModels.SearchModels.Cashier;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize(Roles = Role)]
    public class ProductsController : BaseCashierController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int StoreId, [FromQuery] ProductSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            try
            {
                //check storeId
                paging = PagingUtil.checkDefaultPaging(paging);
                var products = await _productService.GetProductList(StoreId, searchModel, paging);
                return Ok(products);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int StoreId, int id)
        {
            try
            {
                var product = await _productService.GetProductById(StoreId, id);
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
      
        [HttpPost("unpack")]
        public async Task<IActionResult> UnpackProduct(UnpackProductRequestModel model)
        {
            try
            {
                if (model.StoreId <= 0
                    || model.ProductId <= 0)
                {
                    return BadRequest();
                }
                await _productService.UnpackProduct(model.ProductId, model.Quantity, model.StoreId);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}

