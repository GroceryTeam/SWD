using AutoMapper;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.Cashier;
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
namespace BusinessLayer.Services.Cashier
{
    public class BillService : BaseService, IBillService
    {
        public BillService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<int> AddBill(int storeId, int cashierId, BillCreateModel model)
        {
            var mappedBill = _mapper.Map<BillCreateModel, Bill>(model);
            mappedBill.StoreId = storeId;
            mappedBill.CashierId = cashierId;
            mappedBill.DateCreated = DateTime.Now;

            foreach (var detail in model.Details)
            {
                var productInDetail = _unitOfWork.ProductRepository
                    .Get()
                    .Where(x => x.Id == detail.ProductId)
                    .Include(x=>x.Stocks)
                    .FirstOrDefault();
                var correspondingStock = productInDetail.Stocks
                    .Where(x => x.StoreId == storeId)
                    .Where(x => x.ProductId == productInDetail.Id)
                    .FirstOrDefault();
                mappedBill.BillDetails.Add(new BillDetail
                {
                    ProductId = detail.ProductId,
                    BillId = mappedBill.Id,
                    BuyPrice = productInDetail.BuyPrice,
                    SellPrice = correspondingStock.Price,
                    Quantity = detail.Quantity
                });
                correspondingStock.Quantity -= detail.Quantity;
                _unitOfWork.StockRepository.Update(correspondingStock);
            }
            await _unitOfWork.BillRepository.Add(mappedBill);
            await _unitOfWork.SaveChangesAsync();

            return mappedBill.Id;

        }


    }
}
