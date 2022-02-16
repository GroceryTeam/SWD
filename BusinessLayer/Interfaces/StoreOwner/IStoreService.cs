using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
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
    public interface IStoreService
    {
        Task<List<StoreViewModel>> GetStoreList(int brandId);
        Task CreateStore(StoreCreateModel model);
        Task<StoreViewModel> GetStoreById(int storeId);
        Task<bool> UpdateStore(int storeId, StoreCreateModel model);
        Task DeleteStore(int storeId);
    }
}
