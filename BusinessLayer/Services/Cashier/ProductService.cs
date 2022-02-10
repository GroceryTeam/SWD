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
using BusinessLayer.ResponseModels.ViewModels.Cashier;
using BusinessLayer.RequestModels.SearchModels.Cashier;
using static DataAcessLayer.Models.Event;
using AutoMapper;

namespace BusinessLayer.Services.Cashier
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<BasePagingViewModel<ProductViewModel>> GetProductList(int storeId, ProductSearchModel searchModel, PagingRequestModel paging)
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
                         .Where(x => x.Status == Product.ProductStatus.Selling)
                         .ToList();
            //apply event
            //map thuong
            var products = productsData.Select
                                (x => new ProductViewModel()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    UnpackedProductName = x.UnpackedProduct.Name,
                                    OriginalPrice = x.SellPrice,
                                    EventPrice = x.Stocks
                                    .Where(a => a.StoreId == storeId)
                                    .Where(a => a.ProductId == x.Id)
                                    .FirstOrDefault().Price,
                                    CategoryId = x.CategoryId,
                                    ConversionRate = x.ConversionRate,
                                    UnitLabel = x.UnitLabel,
                                }
                                ).ToList();

        int totalItem = productsData.Count;

            productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var productResult = new BasePagingViewModel<ProductViewModel>()
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
