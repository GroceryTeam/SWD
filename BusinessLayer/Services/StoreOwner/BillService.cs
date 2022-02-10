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
    public class BillService : BaseService, IBillService
    {
        public BillService(IUnitOfWork unitOfWork) : base(unitOfWork){}

        public async Task<BasePagingViewModel<BillsViewModel>> GetBills(int storeId, PagingRequestModel paging)
        {
            var billData = await _unitOfWork.BillRepository.Get()
                                    .Where(bill => bill.StoreId == storeId)
                                    .Select(bill => new BillsViewModel()
                                    {
                                        Id = bill.Id,
                                        CashierId = bill.CashierId,
                                        StoreId = bill.StoreId,
                                        DateCreated = bill.DateCreated,
                                        TotalPrice = bill.TotalPrice
                                    }).ToListAsync();

            int totalCount = billData.Count();

            billData = billData.Skip((paging.PageIndex - 1) * paging.PageSize)
                                .Take(paging.PageSize).ToList();

            var billResult = new BasePagingViewModel<BillsViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalCount,
                Data = billData,
                TotalPage = (int)Math.Ceiling((decimal)totalCount / (decimal)paging.PageSize),
            };

            return billResult;
        }

        public async Task<BillsViewModel> GetBillById(int billId)
        {
            var details = await _unitOfWork.BillDetailRepository
                .Get().Where(bd => bd.BillId == billId)
                .Include(bd => bd.Product)
                .Select(bd => new BillDetailsViewModel()
                {
                    BuyPrice = bd.BuyPrice, 
                    SellPrice = bd.SellPrice,
                    Quantity = bd.Quantity,
                    Product = new ProductsViewModel()
                    { 
                        Id = bd.Product.Id,
                        Name = bd.Product.Name,
                        CategoryId = bd.Product.CategoryId,
                        UnitLabel = bd.Product.UnitLabel
                    },
                })
                .ToListAsync();

            var bill = await _unitOfWork.BillRepository.Get()
                .Include(bill => bill.Cashier)
                .Include(bill => bill.BillDetails)
                .ThenInclude(bd => bd.Product)
                .IgnoreAutoIncludes()
                .Select(bill => new BillsViewModel() { 
                    Id = bill.Id,
                    StoreId = bill.StoreId,
                    CashierId = bill.CashierId,
                    DateCreated = bill.DateCreated,
                    TotalPrice = bill.TotalPrice,
                    BillDetails = details,
                    Cashier = bill.Cashier
                })
                .FirstOrDefaultAsync();
            return bill;
        }
    }
}
