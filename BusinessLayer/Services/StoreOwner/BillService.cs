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
            /*var billData = await _unitOfWork.BillRepository.Get()
                                    .Where(bill => bill.StoreId == storeId)
                                    .Select(bill => new BillViewModel()
                                    {
                                        Id = bill.Id,
                                        CashierId = bill.CashierId,
                                        StoreId = bill.StoreId,
                                        DateCreated = bill.DateCreated,
                                        TotalPrice = bill.TotalPrice
                                    }).ToListAsync();

            int totalCount = billData.Count();*/

            var billsData = await _unitOfWork.BillRepository
                .Get()
                .Where(x => x.StoreId == storeId)
                .ToListAsync();

            var mappedBillsData = _mapper.Map<List<Bill>, List<BillViewModel>>(billsData);
            mappedBillsData = mappedBillsData
                .Where(x => (x.DateCreated >= searchModel.StartDate) && (x.DateCreated <= searchModel.EndDate))
                .ToList();

            int totalCount = mappedBillsData.Count();

            mappedBillsData = mappedBillsData.Skip((paging.PageIndex - 1) * paging.PageSize)
                                .Take(paging.PageSize).ToList();

            var billResult = new BasePagingViewModel<BillViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalCount,
                Data = mappedBillsData,
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
                    Product = new ProductViewModel()
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
                .Where(x => x.Id == billId)
                //.IgnoreAutoIncludes()
                .Select(bill => new BillViewModel() { 
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
