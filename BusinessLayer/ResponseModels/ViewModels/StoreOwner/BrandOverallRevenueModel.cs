using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class DailyBrandRevenueDetailModel
    {
        public DateTime Date { get; set; }
        public int Revenue { get; set; }
    }
    public class MonthlyBrandRevenueDetailModel
    {
        public DateTime Month { get; set; }
        public int Revenue { get; set; }
    }

    public class BrandOverallRevenueModel
    {
        public int BrandId { get; set; }
        public int TotalBrandRevenue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DailyBrandRevenueDetailModel> DailyBrandRevenueDetails { get; set; }
        public List<MonthlyBrandRevenueDetailModel> MonthlyBrandRevenueDetails { get; set; }
    }
}
