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
    public interface IStockService
    {
        Task<StockViewModel> GetStockById(int stockId);
        Task<bool> UpdateStock(int stockId, StockEditModel model);
        Task<BasePagingViewModel<ProductStockViewModel>> GetProductIncludingStock(ProductStockSearchModel searchModel, PagingRequestModel paging);
    }
}
