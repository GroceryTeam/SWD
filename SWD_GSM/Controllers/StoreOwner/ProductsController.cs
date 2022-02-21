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

namespace SWD_GSM.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    [ApiExplorerSettings(GroupName = Role)]
    [Authorize]
    [Authorize(Roles = "StoreOwner")]

    public class ProductsController : BaseStoreOwnerController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService service)
        {
            _productService = service;
        }


        [HttpGet]
        public async Task<IActionResult> Get(int BrandId, [FromQuery] ProductSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            try
            {
                //check storeId
                paging = PagingUtil.checkDefaultPaging(paging);
                var products = await _productService.GetProductList(BrandId, searchModel, paging);
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int BrandId, int id)
        {
            try
            {
                var product = await _productService.GetProductById(BrandId, id);
                if (product!=null)
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
        public async Task<IActionResult> CreateNewProduct(int BrandId, [FromBody] ProductCreateModel model)
        {
            try
            {
                if (model.ConversionRate <= 0 
                    || model.SellPrice < 0 
                    || model.LowerThreshold < 0)
                {
                    return BadRequest();
                }
                var id = await _productService.AddProduct(BrandId, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int BrandId, int id, [FromBody] ProductCreateModel model)
        {
            try
            {
                if (model.ConversionRate <= 0 || model.SellPrice < 0 || model.LowerThreshold < 0)
                {
                    return BadRequest();
                }
                await _productService.UpdateProduct(BrandId, id, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int BrandId, int id)
        {
            try
            {
                var result = await _productService.DeleteProduct(BrandId,id);
                if (result.InverseUnpackedProducts.Count==0)
                {
                    return NoContent();
                }else
                {
                    return Conflict(result);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
       
        //[NonAction]
        //private PagingRequestModel getDefaultPaging()
        //{
        //    return new PagingRequestModel
        //    {
        //        PageIndex = PageConstant.DefaultPageIndex,
        //        PageSize = PageConstant.DefaultPageSize
        //    };
        //}
        //[NonAction]
        //private PagingRequestModel checkDefaultPaging(PagingRequestModel paging)
        //{
        //    if (paging.PageIndex <= 0) paging.PageIndex = PageConstant.DefaultPageIndex;
        //    if (paging.PageSize <= 0) paging.PageSize = PageConstant.DefaultPageSize;
        //    return paging;
        //}
    }
}

