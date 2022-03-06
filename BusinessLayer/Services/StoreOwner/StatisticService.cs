using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.Services;
using DataAcessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.StoreOwner
{
    public class StatisticService : BaseService, IStatisticService
    {
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        private IEnumerable<DateTime> GetEveryDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1))
                yield return date;
        }
        private IEnumerable<DateTime> GetEveryMonth(DateTime startDate, DateTime endDate)
        {
            var firstDayOfStartMonth = new DateTime(startDate.Year, startDate.Month, 1);
           
            var lastDayOfEndMonth = new DateTime(endDate.Year, endDate.Month, 1)
                                        .AddMonths(1)
                                        .AddDays(-1);
            for (var date = firstDayOfStartMonth; date.Date <= lastDayOfEndMonth; date = date.AddMonths(1))
                yield return date;
        }
        public BrandOverallRevenueModel GetBrandRevenue(RevenueSearchModel model)
        {
            BrandOverallRevenueModel result = new BrandOverallRevenueModel()
            {
                BrandId = (int)model.BrandId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TotalBrandRevenue = 0,
            };
            var billIdList = _unitOfWork.BillRepository
                .Get()
                .Include(x=>x.Store)
                .ThenInclude(x=>x.Brand)
                .Where(x=>x.Store.Brand.Id==model.BrandId)
                .Where(x => x.DateCreated <= model.EndDate && x.DateCreated >= model.StartDate)
                .Select(x => x.Id)
                .ToList();

            var billdetailList = _unitOfWork.BillDetailRepository
                .Get()
                .Include(x=>x.Bill)
                .ToList();
            billdetailList = billdetailList.Where(x => billIdList.Contains(x.Id)).ToList();

            if (model.ReturnDateResult)
            {
                result.DailyBrandRevenueDetails = new List<DailyBrandRevenueDetailModel>();
                foreach(DateTime day in GetEveryDay(model.StartDate,model.EndDate))
                {
                    result.DailyBrandRevenueDetails.Add(new DailyBrandRevenueDetailModel()
                    {
                        Date = day,
                        Revenue = 0
                    });
                }
                billdetailList.ForEach(_detail =>
                {
                    var _detailWithTheRightDate = result.DailyBrandRevenueDetails
                            .Where(x => x.Date.Date == _detail.Bill.DateCreated.Date)
                            .FirstOrDefault();
                    _detailWithTheRightDate.Revenue += (_detail.SellPrice - _detail.BuyPrice)*_detail.Quantity;
                });
            }
            else if (model.ReturnMonthResult)
            {
                result.MonthlyBrandRevenueDetails = new List<MonthlyBrandRevenueDetailModel>();
                foreach (DateTime day in GetEveryMonth(model.StartDate, model.EndDate))
                {
                    result.MonthlyBrandRevenueDetails.Add(new MonthlyBrandRevenueDetailModel()
                    {
                        Month = day,
                        Revenue = 0
                    });
                }
                billdetailList.ForEach(_detail =>
                {
                    var _detailWithTheRightDate = result.MonthlyBrandRevenueDetails
                            .Where(x => x.Month.Date.Month == _detail.Bill.DateCreated.Date.Month)
                            .FirstOrDefault();
                    _detailWithTheRightDate.Revenue += (_detail.SellPrice - _detail.BuyPrice) * _detail.Quantity;
                });
            }
            return result;
        }

        public StoreOverallRevenueModel GetStoreRevenue(RevenueSearchModel model)
        {
            StoreOverallRevenueModel result = new StoreOverallRevenueModel()
            {
                StoreId = (int)model.StoreId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TotalStoreRevenue = 0,
            };
            var billIdList = _unitOfWork.BillRepository
                .Get()
                .Include(x => x.Store)
                .Where(x => x.Store.Id == model.StoreId)
                .Where(x => x.DateCreated <= model.EndDate && x.DateCreated >= model.StartDate)
                .Select(x => x.Id)
                .ToList();

            var billdetailList = _unitOfWork.BillDetailRepository
                .Get()
                .Include(x => x.Bill)
                .ToList();
            billdetailList = billdetailList.Where(x => billIdList.Contains(x.Id)).ToList();

            if (model.ReturnDateResult)
            {
                result.DailyStoreRevenueDetails = new List<DailyStoreRevenueDetailModel>();
                foreach (DateTime day in GetEveryDay(model.StartDate, model.EndDate))
                {
                    result.DailyStoreRevenueDetails.Add(new DailyStoreRevenueDetailModel()
                    {
                        Date = day,
                        Revenue = 0
                    });
                }
                billdetailList.ForEach(_detail =>
                {
                    var _detailWithTheRightDate = result.DailyStoreRevenueDetails
                            .Where(x => x.Date.Date == _detail.Bill.DateCreated.Date)
                            .FirstOrDefault();
                    _detailWithTheRightDate.Revenue += (_detail.SellPrice - _detail.BuyPrice) * _detail.Quantity;
                });
            }
            else if (model.ReturnMonthResult)
            {
                result.MonthlyStoreRevenueDetails = new List<MonthlyStoreRevenueDetailModel>();
                foreach (DateTime day in GetEveryMonth(model.StartDate, model.EndDate))
                {
                    result.MonthlyStoreRevenueDetails.Add(new MonthlyStoreRevenueDetailModel()
                    {
                        Month = day,
                        Revenue = 0
                    });
                }
                billdetailList.ForEach(_detail =>
                {
                    var _detailWithTheRightDate = result.MonthlyStoreRevenueDetails
                            .Where(x => x.Month.Date.Month == _detail.Bill.DateCreated.Date.Month)
                            .FirstOrDefault();
                    _detailWithTheRightDate.Revenue += (_detail.SellPrice - _detail.BuyPrice) * _detail.Quantity;
                });
            }
            return result;
        }
    }
}
