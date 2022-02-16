using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.SystemAdmin
{
    public interface IStoreService
    {
        Task<BasePagingViewModel<StoreViewModel>> GetStoreList(StoreSearchModel searchModel, PagingRequestModel paging);
        Task<StoreViewModel> GetStoreById(int storeId);
        Task ChangeStoreStatus(int storeId, int status);
    }
}
