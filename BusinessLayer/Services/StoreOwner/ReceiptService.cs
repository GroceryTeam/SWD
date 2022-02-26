using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
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
    public class ReceiptService : BaseService, IReceiptService
    {
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<ReceiptViewModel> GetReceiptById(int storeId, int receiptId)
        {
            var _receipt = await _unitOfWork.ReceiptRepository
              .Get()
              .Include(_receipt => _receipt.ReceiptDetails)
              .ThenInclude(x => x.Product)
              .Where(receipt => receipt.StoreId == storeId)
              .Where(receipt => receipt.Id == receiptId)
              .Select(receipt => new ReceiptViewModel()
              {
                  Id = receipt.Id,
                  StoreId = receipt.StoreId,
                  DateCreated = receipt.DateCreated,
                  TotalCost = receipt.TotalCost,
                  ReceiptDetails =
                        (List<ReceiptDetailViewModel>)receipt.ReceiptDetails.Select
                        (x => new ReceiptDetailViewModel()
                        {
                            ReceiptId = x.ReceiptId,
                            ProductId = x.ProductId,
                            ProductName = x.Product.Name,
                            BuyPrice = x.BuyPrice,
                            Quantity = x.Quantity
                        })
              }
              ).FirstOrDefaultAsync();
            return _receipt;
        }

        public async Task<BasePagingViewModel<ReceiptViewModel>> GetReceiptList(int storeId, ReceiptSearchModel model, PagingRequestModel paging)
        {
            var receiptData = await _unitOfWork.ReceiptRepository.Get()
                                    .Include(_receipt => _receipt.ReceiptDetails)
                                    .ThenInclude(x => x.Product)
                                    .Where(receipt => receipt.StoreId == storeId)
                                    .Select(receipt => new ReceiptViewModel()
                                    {
                                        Id = receipt.Id,
                                        StoreId = receipt.StoreId,
                                        DateCreated = receipt.DateCreated,
                                        TotalCost = receipt.TotalCost,
                                        ReceiptDetails =
                                            (List<ReceiptDetailViewModel>)receipt.ReceiptDetails.Select
                                            (x => new ReceiptDetailViewModel()
                                            {
                                                ReceiptId = x.ReceiptId,
                                                ProductId = x.ProductId,
                                                ProductName = x.Product.Name,
                                                BuyPrice = x.BuyPrice,
                                                Quantity = x.Quantity
                                            })
                                    }).ToListAsync();

            receiptData = receiptData
                .Where(x => (x.DateCreated >= model.StartDate) && (x.DateCreated <= model.EndDate))
                .ToList();

            int totalCount = receiptData.Count();

            receiptData = receiptData.Skip((paging.PageIndex - 1) * paging.PageSize)
                                .Take(paging.PageSize).ToList();

            var receiptResult = new BasePagingViewModel<ReceiptViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalCount,
                TotalPage = (int)Math.Ceiling((decimal)totalCount / (decimal)paging.PageSize),
                Data = receiptData
            };

            return receiptResult;
        }

        public async Task AddReceipt( ReceiptCreateModel model)
        {
            var receipt = new Receipt()
            {
                StoreId = model.StoreId,
                TotalCost = model.TotalCost,
                DateCreated = DateTime.Now,
                ReceiptDetails = new List<ReceiptDetail>()
            };
            await _unitOfWork.ReceiptRepository.Add(receipt);
            await _unitOfWork.SaveChangesAsync();

            foreach (var _detail in model.Details)
            {
                var detailModel = new ReceiptDetail()
                {
                    ProductId = _detail.ProductId,
                    Quantity = _detail.Quantity,
                    ReceiptId = receipt.Id,
                    BuyPrice = _detail.BuyPrice,
                };
                receipt.ReceiptDetails.Add(detailModel);
                var stock = new Stock()
                {
                    ProductId = _detail.ProductId,
                    StoreId = receipt.StoreId,
                    Quantity = _detail.Quantity,
                    BuyPrice = _detail.BuyPrice,
                    Status = (_unitOfWork.StockRepository.Get().Where(x => x.StoreId == model.StoreId)
                                                           .Where(x => x.ProductId == _detail.ProductId)
                                                           .Where(x => x.Status == Stock.StockDetail.Selling)
                                                           .FirstOrDefault()
                                                           != null)
                                                               ? Stock.StockDetail.Available
                                                               : Stock.StockDetail.Selling,
                     ReceiptId = receipt.Id,
                };
                await _unitOfWork.StockRepository.Add(stock);
                await _unitOfWork.SaveChangesAsync();

                int remainingQuantityInStore = 0;
                _unitOfWork.StockRepository.Get()
                    .Where(x => x.ProductId == _detail.ProductId)
                    .Where(x => x.StoreId == model.StoreId)
                    .ToList().ForEach(x=>remainingQuantityInStore+=x.Quantity);
                var product = await _unitOfWork.ProductRepository.Get().Where(x => x.Id == _detail.ProductId).FirstOrDefaultAsync();
                if (remainingQuantityInStore > product.LowerThreshold)
                {
                    product.Status = Product.ProductStatus.Selling;
                }
            }
            _unitOfWork.ReceiptRepository.Update(receipt);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
