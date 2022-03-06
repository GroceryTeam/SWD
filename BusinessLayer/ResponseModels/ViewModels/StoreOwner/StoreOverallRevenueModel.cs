using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class DailyStoreRevenueDetailModel
    {
        public DateTime Date { get; set; }
        public int Revenue { get; set; }
    }
    public class MonthlyStoreRevenueDetailModel
    {
        public DateTime Month { get; set; }
        public int Revenue { get; set; }
    }
    public class StoreOverallRevenueModel
    {
        public int StoreId { get; set; }
        public int TotalStoreRevenue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DailyStoreRevenueDetailModel> DailyStoreRevenueDetails { get; set; }
        public List<MonthlyStoreRevenueDetailModel> MonthlyStoreRevenueDetails { get; set; }
    }
}
