using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
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
    public class BillService : BaseService, IBillService
    {
        public BillService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<BasePagingViewModel<BillViewModel>> GetBills(int storeId, BillSearchModel searchModel, PagingRequestModel paging)
        {
            var billData = await _unitOfWork.BillRepository.Get()
                                    .Where(bill => bill.StoreId == storeId)
                                    .Include(x => x.Cashier)
                                    .Include(x => x.BillDetails)
                                    .ThenInclude(x => x.Product)
                                    .Select(bill => new BillViewModel()
                                    {
                                        Id = bill.Id,
                                        CashierId = bill.CashierId,
                                        StoreId = bill.StoreId,
                                        DateCreated = bill.DateCreated,
                                        TotalPrice = bill.TotalPrice,
                                        CashierName = bill.Cashier.Name,
                                        BillDetails = 
                                        (List<BillDetailViewModel>)bill.BillDetails
                                        .Select(detail => new BillDetailViewModel()
                                        {
                                            BuyPrice = detail.BuyPrice,
                                            SellPrice = detail.SellPrice,
                                            Quantity = detail.Quantity,
                                            ProductId = detail.ProductId,
                                            ProductName = detail.Product.Name,
                                            StockId = detail.StockId
                                        })
                                    })
                                    .ToListAsync();

            
            //var billsData = await _unitOfWork.BillRepository
            //    .Get()
            //    .Where(x => x.StoreId == storeId)
            //    .ToListAsync();

            //var mappedBillsData = _mapper.Map<List<Bill>, List<BillViewModel>>(billsData);
            //mappedBillsData.ForEach(x=>)'
            billData = billData
                .Where(x => (x.DateCreated >= searchModel.StartDate) && (x.DateCreated <= searchModel.EndDate))
                .ToList();


            int totalCount = billData.Count();

            billData = billData.Skip((paging.PageIndex - 1) * paging.PageSize)
                                .Take(paging.PageSize).ToList();

            var billResult = new BasePagingViewModel<BillViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalCount,
                Data = billData,
                TotalPage = (int)Math.Ceiling((decimal)totalCount / (decimal)paging.PageSize),
            };

            return billResult;
        }

        public async Task<BillViewModel> GetBillById(int billId)
        {
            var details = await _unitOfWork.BillDetailRepository
                .Get().Where(bd => bd.BillId == billId)
                .Include(bd => bd.Product)
                .Select(bd => new BillDetailViewModel()
                {
                    BuyPrice = bd.BuyPrice, 
                    SellPrice = bd.SellPrice,
                    Quantity = bd.Quantity,
                    ProductName = bd.Product.Name,
                    ProductId = bd.Product.Id,
                    StockId = bd.StockId
                })
                .ToListAsync();

            var bill = await _unitOfWork.BillRepository.Get()
                .Include(bill => bill.Cashier)
                .Include(bill => bill.BillDetails)
                .ThenInclude(bd => bd.Product)
                .Where(x => x.Id == billId)
                //.IgnoreAutoIncludes()
                .Select(bill => new BillViewModel() { 
                    Id = bill.Id,
                    StoreId = bill.StoreId,
                    CashierId = bill.CashierId,
                    DateCreated = bill.DateCreated,
                    TotalPrice = bill.TotalPrice,
                    BillDetails = details,
                    CashierName = bill.Cashier.Name
                })
                .FirstOrDefaultAsync();
            return bill;
        }


    }
}
