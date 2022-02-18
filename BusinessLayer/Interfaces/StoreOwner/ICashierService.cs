using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.StoreOwner
{
    public interface ICashierService
    {
        Task<BasePagingViewModel<CashierViewModel>> GetCashierList(CashierSearchModel searchModel, PagingRequestModel paging);
        Task<int> AddCashier(int storeId, CashierCreateModel model);
        Task<bool> UpdateCashier(int cashierId, CashierCreateModel model);
        Task DeleteCashier(int cashierId); 
        Task<CashierViewModel> GetCashierById(int cashierId);

    }
}
