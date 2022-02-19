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
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using static DataAcessLayer.Models.Product;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using AutoMapper;

namespace BusinessLayer.Services.StoreOwner
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<BasePagingViewModel<ProductViewModel>> GetProductList(int brandId, ProductSearchModel searchModel, PagingRequestModel paging)
        {
            var productsData = await _unitOfWork.ProductRepository
                .Get()
                .Where(x => x.BrandId == brandId)
                .Select
                (x => new ProductViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnpackedProductId = x.UnpackedProductId,
                    UnpackedProductName = x.UnpackedProduct.Name,
                    SellPrice = x.SellPrice,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    ConversionRate = (int)x.ConversionRate,
                    UnitLabel = x.UnitLabel,
                    LowerThreshold = x.LowerThreshold,
                    Status = (int)x.Status
                }
                )
                .ToListAsync();
            //var mappedProductsData = _mapper.Map<List<Product>, List<ProductViewModel>>(productsData);
            //mappedProductsData.ForEach(x => x.Status = (int)x.Status);

            productsData = productsData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .Where(x => (searchModel.MinimumSellingPrice != null)
                                            ? x.SellPrice >= searchModel.MinimumSellingPrice
                                            : true)
                        .Where(x => (searchModel.MaximumSellingPrice != null)
                                            ? x.SellPrice <= searchModel.MaximumSellingPrice
                                            : true)
                        .Where(x => (searchModel.Status != null)
                                            ? x.Status == (int)searchModel.Status
                                            : true)
                         .Where(x => (searchModel.CategoryId != null)
                                            ? x.CategoryId == searchModel.CategoryId
                                            : true)
                        .ToList();

            int totalItem = productsData.Count;

            productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var productResult = new BasePagingViewModel<ProductViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = productsData
            };
            return productResult;
        }
        public async Task<ProductViewModel> GetProductById(int brandId, int productId)
        {
            var product = await _unitOfWork.ProductRepository
              .Get()
              .Where(x => x.BrandId == brandId)
              .Where(x => x.Id == productId)
              .FirstOrDefaultAsync();
            if (product != null)
            {
                var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
                mappedProduct.Status = (int)product.Status;
                return mappedProduct;
            }
            return null;
        }
        public async Task<int> AddProduct(int brandId, ProductCreateModel model)
        {
            //var product = new Product()
            //{
            //    Name = model.Name,
            //    UnpackedProductId = model.UnpackedProductId,
            //    BuyPrice = model.BuyPrice,
            //    SellPrice = model.SellPrice,
            //    CategoryId = model.CategoryId,
            //    ConversionRate = model.ConversionRate,
            //    UnitLabel = model.UnitLabel,
            //    BrandId = brandId,
            //    LowerThreshold = model.LowerThreshold,
            //    Status = Product.ProductStatus.Selling
            //};
            var mappedProduct = _mapper.Map<ProductCreateModel, Product>(model);
            mappedProduct.BrandId = brandId;
            mappedProduct.Status = Product.ProductStatus.Selling;

            await _unitOfWork.ProductRepository.Add(mappedProduct);
            await _unitOfWork.SaveChangesAsync();

            mappedProduct = await _unitOfWork.Context().Products
                .Where(x => x.Id == mappedProduct.Id)
                .Include(x => x.Brand)
                .ThenInclude(x => x.Stores)
                .FirstOrDefaultAsync();
            //foreach (Store store in mappedProduct.Brand.Stores)
            //{
            //    var stock = new Stock()
            //    {
            //        Price = mappedProduct.SellPrice,
            //        Product = mappedProduct,
            //        ProductId = mappedProduct.Id,
            //        Quantity = 0,
            //        Status = Stock.StockDetail.NearlyOutOfStock,
            //        StoreId = store.Id
            //    };
            //    await _unitOfWork.StockRepository.Add(stock);
            //}
            await _unitOfWork.SaveChangesAsync();

            return mappedProduct.Id;
        }
        public async Task<bool> UpdateProduct(int brandId, int productId, ProductCreateModel model)
        {
            var product = await _unitOfWork.ProductRepository.Get()
                .Where(x => x.BrandId.Equals(brandId))
                .Where(x => x.Id.Equals(productId))
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return false;
            }

            product.Name = model.Name;
            product.UnpackedProductId = model.UnpackedProductId;
            product.SellPrice = model.SellPrice;
            product.CategoryId = model.CategoryId;
            product.ConversionRate = model.ConversionRate;
            product.UnitLabel = model.UnitLabel;
            product.LowerThreshold = model.LowerThreshold;
            product.Status = model.Status;

            //product = _mapper.Map<ProductCreateModel, Product>(model);
            //product.BrandId = brandId;
            //product.Id = productId;


            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<DeleteProductErrorModel> DeleteProduct(int brandId, int productId)
        {
            var product = await _unitOfWork.ProductRepository.Get()
                .Where(x => x.BrandId.Equals(brandId))
                .Where(x => x.Id.Equals(productId))
                .Include(x => x.InverseUnpackedProduct)
                .FirstOrDefaultAsync();
            var result = new DeleteProductErrorModel()
            {
                InverseUnpackedProducts = new List<ProductErrorModel>()
            };
            if (product == null)
            {
                throw new Exception();
            }
            foreach (Product p in product.InverseUnpackedProduct)
            {
                if (p.Status == ProductStatus.Selling)
                    result.InverseUnpackedProducts
                    .Add(new ProductErrorModel
                    {
                        Id = p.Id,
                        Name = p.Name
                    }
                    );
            }
            if (result.InverseUnpackedProducts.Count == 0)
            {
                product.Status = ProductStatus.Disabled;
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }


    }
}
