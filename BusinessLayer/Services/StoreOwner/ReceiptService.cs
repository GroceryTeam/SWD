using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
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

        public async Task<BasePagingViewModel<ReceiptViewModel>> GetReceiptList(int storeId, PagingRequestModel paging)
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
    }
}
