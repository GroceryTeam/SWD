using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
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
    public interface IEventService
    {
        Task<EventsViewModel> GetEventById(int brandId, int productId);
        Task<BasePagingViewModel<EventsViewModel>> GetEventList(int brandId, EventSearchModel searchModel, PagingRequestModel paging);
    }
}
