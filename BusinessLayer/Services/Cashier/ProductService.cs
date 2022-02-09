using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.Services;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.ResponseModel.ViewModels.Cashier;
using BusinessLayer.RequestModels.SearchModels.Cashier;
using static DataAcessLayer.Models.Event;

namespace BusinessLayer.Services.Cashier
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<BasePagingViewModel<ProductsViewModel>> GetProductList(int storeId, ProductSearchModel searchModel, PagingRequestModel paging)
        {
            var productsData = await _unitOfWork.ProductRepository
                .Get()
                 .Include(x => x.Stocks)
                 .Include(x => x.InverseUnpackedProduct)
               .ToListAsync();
            productsData = productsData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                         .Where(x => (searchModel.CategoryId != null)
                                            ? x.CategoryId == searchModel.CategoryId
                                            : true)
                         .ToList();
            //apply event
            var products = productsData.Select
                                (x => new ProductsViewModel()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    UnpackedProductName = x.UnpackedProduct.Name,
                                    OriginalPrice = x.SellPrice,
                                    EventPrice = x.Stocks.Where(x => x.StoreId == storeId).FirstOrDefault().Price,
                                    CategoryId = x.CategoryId,
                                    ConversionRate = x.ConversionRate,
                                    UnitLabel = x.UnitLabel,
                                }
                                ).ToList();

            int totalItem = productsData.Count;

            productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var productResult = new BasePagingViewModel<ProductsViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = products
            };
            return productResult;
        }
    }
}
