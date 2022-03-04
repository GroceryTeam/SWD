using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
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
namespace BusinessLayer.Services.StoreOwner
{
    public class StockService : BaseService, IStockService
    {
        public StockService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<BasePagingViewModel<ProductStockViewModel>> GetProductIncludingStock(ProductStockSearchModel searchModel, PagingRequestModel paging)
        {
            var productsList = await _unitOfWork.ProductRepository
                .Get()
                .Include(x => x.UnpackedProduct)
                .Include(x => x.Category)
                .Include(x => x.Brand).ThenInclude(x => x.Stores)
                .ToListAsync();
            var productsData = productsList
                        .Where(x => x.BrandId == searchModel.BrandId)
                        .Where(x => StringNormalizer.VietnameseNormalize(x.Name)
                                   .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                       .Where(x => searchModel.StoreId == null
                                            || x.Brand.Stores.Select(x => x.Id).ToList().Contains((int)searchModel.StoreId))
                       .Where(x => searchModel.ProductId == null
                                            || x.Id == searchModel.ProductId)
                       .Where(x => (searchModel.CategoryId != null)
                                           ? x.CategoryId == searchModel.CategoryId
                                           : true)
                       .Where(x => (!string.IsNullOrEmpty(searchModel.Sku))
                                           ? x.Sku == searchModel.Sku
                                           : true)
                       .Where(x => (searchModel.OnlyNearlyOutOfStockProduct)
                                           ? x.Status == Product.ProductStatus.NearlyOutOfStock
                                           : true)
                       .Select(x => new ProductStockViewModel()
                       {

                           Id = x.Id,
                           CategoryId = x.CategoryId,
                           CategoryName = x.Category.Name,
                           Name = x.Name,
                           ConversionRate = (int)x.ConversionRate,
                           LowerThreshold = (int)x.LowerThreshold,
                           SellPrice = x.SellPrice,
                           Status = (int)x.Status,
                           UnitLabel = x.UnitLabel,
                           BrandId = x.BrandId,
                           Sku=x.Sku,
                           //init current quantity, add later
                           CurrentQuantity = 0,
                           UnpackedProductName = x.UnpackedProduct != null ? x.UnpackedProduct.Name : null,
                           Stocks = new List<StockViewModel>()
                       }).ToList();

            var stocksData = await _unitOfWork.StockRepository
                .Get()
                .Include(x => x.Product)
                .ThenInclude(x => x.Category)
                .Include(x => x.Store)
                .ThenInclude(x => x.Brand)
                .ToListAsync();


            stocksData
                        .Where(x => x.Store.Brand.Id == searchModel.BrandId)
                       .Where(x =>
                           StringNormalizer.VietnameseNormalize(x.Product.Name)
                           .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                       .Where(x => (searchModel.StoreId != null)
                                           ? x.StoreId == searchModel.StoreId
                                           : true)
                       .Where(x => (searchModel.ProductId != null)
                                           ? x.ProductId == (searchModel.ProductId)
                                           : true)
                       .Where(x => (searchModel.CategoryId != null)
                                           ? x.Product.CategoryId == searchModel.CategoryId
                                           : true)
                       .Where(x => (searchModel.OnlyNearlyOutOfStockProduct)
                                           ? x.Product.Status == Product.ProductStatus.NearlyOutOfStock
                                           : true)
                       .ToList().ForEach(stck =>
                             {
                                 if (stck.Quantity > 0 && stck.Status != Stock.StockDetail.SoldOut)
                                 {
                                     var product = productsData
                                              .Where(product => product.Id == stck.ProductId)
                                              .FirstOrDefault();
                                     product.Stocks.Add(new StockViewModel()
                                     {
                                         Id = stck.Id,
                                         ProductId = product.Id,
                                         Quantity = stck.Quantity,
                                         ReceiptId = stck.ReceiptId,
                                         StoreId = stck.StoreId,
                                         BuyPrice = stck.BuyPrice,
                                         Status = (int)stck.Status
                                     });
                                     product.CurrentQuantity += stck.Quantity;
                                 }
                             });

            int totalItem = productsData.Count;

            productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var productStockResult = new BasePagingViewModel<ProductStockViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = productsData
            };
            return productStockResult;
        }
        public async Task<bool> UpdateStock(int stockId, StockEditModel model)
        {
            var stock = await _unitOfWork.StockRepository.Get()
                .Where(x => x.Id.Equals(stockId))
                .FirstOrDefaultAsync();
            if (stock == null)
            {
                return false;
            }

            stock.Quantity = model.NewQuantity;

            _unitOfWork.StockRepository.Update(stock);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<StockViewModel> GetStockById(int stockId)
        {
            var stock = await _unitOfWork.StockRepository
              .Get()
              .Where(x => x.Id == stockId)
              .Select(stck => new StockViewModel()
              {
                  Id = stck.Id,
                  ProductId = stck.ProductId,
                  Quantity = stck.Quantity,
                  ReceiptId = stck.ReceiptId,
                  StoreId = stck.StoreId,
                  BuyPrice = stck.BuyPrice,
                  Status = (int)stck.Status
              })
              .FirstOrDefaultAsync();
            if (stock != null)
            {
                return stock;
            }
            return null;
        }
    }
}
