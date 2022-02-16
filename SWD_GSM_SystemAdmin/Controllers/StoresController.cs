using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_SystemAdmin.Controllers.SystemAdmin
{
    [Route(SystemAdminRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]

    public class StoresController : BaseSystemAdminController
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int BrandId, StoreSearchModel model, PagingRequestModel paging)
        {
            return null;
            //var productsData = await _unitOfWork.ProductRepository
            //     .Get()
            //     .Where(x => x.BrandId == brandId)
            //     .Select
            //     (x => new ProductViewModel()
            //     {
            //         Id = x.Id,
            //         Name = x.Name,
            //         UnpackedProductId = x.UnpackedProductId,
            //         UnpackedProductName = x.UnpackedProduct.Name,
            //         BuyPrice = x.BuyPrice,
            //         SellPrice = x.SellPrice,
            //         CategoryId = x.CategoryId,
            //         CategoryName = x.Category.Name,
            //         ConversionRate = x.ConversionRate,
            //         UnitLabel = x.UnitLabel,
            //         LowerThreshold = x.LowerThreshold,
            //         Status = (int)x.Status
            //     }
            //     )
            //     .ToListAsync();

            //productsData = productsData
            //            .Where(x =>
            //                StringNormalizer.VietnameseNormalize(x.Name)
            //                .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
            //            .Where(x => (searchModel.MinimumBuyingPrice != null)
            //                                ? x.BuyPrice >= searchModel.MinimumBuyingPrice
            //                                : true)
            //            .Where(x => (searchModel.MaximumBuyingPrice != null)
            //                                ? x.BuyPrice <= searchModel.MaximumBuyingPrice
            //                                : true)
            //            .Where(x => (searchModel.MinimumSellingPrice != null)
            //                                ? x.SellPrice >= searchModel.MinimumSellingPrice
            //                                : true)
            //            .Where(x => (searchModel.MaximumSellingPrice != null)
            //                                ? x.SellPrice <= searchModel.MaximumSellingPrice
            //                                : true)
            //            .Where(x => (searchModel.Status != null)
            //                                ? x.Status == (int)searchModel.Status
            //                                : true)
            //             .Where(x => (searchModel.CategoryId != null)
            //                                ? x.CategoryId == searchModel.CategoryId
            //                                : true)
            //            .ToList();

            //int totalItem = productsData.Count;

            //productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
            //    .Take(paging.PageSize).ToList();

            //var productResult = new BasePagingViewModel<ProductViewModel>()
            //{
            //    PageIndex = paging.PageIndex,
            //    PageSize = paging.PageSize,
            //    TotalItem = totalItem,
            //    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
            //    Data = productsData
            //};
            //return productResult;
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
    }
}

