using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.StoreOwner
{
    public interface IStatisticService
    {
        BrandOverallRevenueModel GetBrandRevenue(RevenueSearchModel model);
        StoreOverallRevenueModel GetStoreRevenue(RevenueSearchModel model);
    }
}
