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
using Microsoft.Extensions.Caching.Distributed;

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
                  .Include(x => x.Brand)
                     .ThenInclude(x => x.Events)
                     .ThenInclude(x => x.EventDetails)
                     .ThenInclude(x => x.Product)
 .Include(x => x.Brand).ThenInclude(x => x.Stores)
                 .Include(x => x.InverseUnpackedProduct)
               .ToListAsync();
            productsData = productsData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                         .Where(x => (searchModel.CategoryId != null)
                                            ? x.CategoryId == searchModel.CategoryId
                                            : true)
                         .Where(x => (!string.IsNullOrEmpty(searchModel.Sku))
                                           ? x.Sku == searchModel.Sku
                                           : true)
                         .Where(x => x.Status != Product.ProductStatus.Disabled)
                         .ToList();

            var currentEvent = (productsData.Count > 0) ? productsData[0].Brand.Events.Where(x => x.Status == EventStatus.Enabled).FirstOrDefault() : null;
            var products = productsData.Select
                                (x =>
                                    {
                                        int eventPrice = x.SellPrice;
                                        if (currentEvent != null)
                                        {
                                            var eventEventProduct = currentEvent.EventDetails.Where(detail => detail.ProductId == x.Id).FirstOrDefault();
                                            if (eventEventProduct != null)
                                            {
                                                eventPrice = eventEventProduct.NewPrice;
                                            }
                                        }
                                        return new ProductViewModel()
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            UnpackedProductName = x.UnpackedProduct != null ? x.UnpackedProduct.Name : null,
                                            OriginalPrice = x.SellPrice,
                                            EventPrice = eventPrice,
                                            CategoryId = x.CategoryId,
                                            ConversionRate = (int)x.ConversionRate,
                                            UnitLabel = x.UnitLabel,
                                            Sku = x.Sku,
                                            Quantity = x.Stocks
                                         .Where(a => a.StoreId == storeId)
                                         .Where(a => a.ProductId == x.Id)
                                         .ToList().Select(x => x.Quantity).Sum()
                                        };
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

        public async Task<ProductViewModel> GetProductById(int storeId, int productId)
        {
            var product = await _unitOfWork.ProductRepository
              .Get()
              .Include(x => x.Stocks)
              .Include(x => x.Brand)
                 .ThenInclude(x => x.Events)
                 .ThenInclude(x => x.EventDetails)
                 .ThenInclude(x => x.Product)
               .Include(x => x.Brand).ThenInclude(x => x.Stores)
              .Include(x => x.InverseUnpackedProduct)
              .Where(x => x.Id == productId)
              .FirstOrDefaultAsync();
            // var currentEvent = product.Brand.Events.Where(x => x.Status == EventStatus.Enabled).FirstOrDefault();
            var currentEvent = (product != null) ? product.Brand.Events.Where(x => x.Status == EventStatus.Enabled).FirstOrDefault() : null;
            if (product != null)
            {
                int eventPrice = product.SellPrice;
                if (currentEvent != null)
                {
                    var eventEventProduct = currentEvent.EventDetails.Where(x => x.ProductId == product.Id).FirstOrDefault();
                    if (eventEventProduct != null)
                    {
                        eventPrice = eventEventProduct.NewPrice;
                    }
                }
                var productData = new ProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    UnpackedProductName = product.UnpackedProduct != null ? product.UnpackedProduct.Name : null,
                    OriginalPrice = product.SellPrice,
                    EventPrice = eventPrice,
                    CategoryId = product.CategoryId,
                    ConversionRate = (int)product.ConversionRate,
                    UnitLabel = product.UnitLabel,
                    Sku = product.Sku,
                    Quantity = product.Stocks
                                    .Where(a => a.StoreId == storeId)
                                    .Where(a => a.ProductId == product.Id)
                                    .ToList().Select(x => x.Quantity).Sum()
                };
                return productData;
            }
            return null;
        }

        public async Task UnpackProduct(int productId, int number, int storeId)
        {
            var product = await _unitOfWork.ProductRepository.Get()
                .Where(x => x.Id == productId)
                .FirstOrDefaultAsync();
            if (product.UnpackedProduct != null)
            {
                var stockOfThisProduct = await _unitOfWork.StockRepository.Get()
                    .Where(x => x.ProductId == productId)
                    .Where(x => x.StoreId == storeId)
                    .Where(x => x.Status == Stock.StockDetail.Selling)
                    .FirstOrDefaultAsync();
                stockOfThisProduct.Quantity -= 1;
                var stockOfUnpackedProduct = await _unitOfWork.StockRepository
                    .Get()
                    .Where(x => x.ProductId == product.UnpackedProductId)
                    .Where(x => x.StoreId == storeId)
                    .Where(x => x.Status == Stock.StockDetail.Selling)
                    .FirstOrDefaultAsync();
                stockOfUnpackedProduct.Quantity += (int)product.ConversionRate;
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
