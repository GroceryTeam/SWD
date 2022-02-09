using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.StoreOwner
{
    public interface IBillService
    {
        public Task<BasePagingViewModel<BillsViewModel>> GetBills(int storeId, PagingRequestModel paging);
        public Task<BillsViewModel> GetBillById(int billId);
    }

    
}
