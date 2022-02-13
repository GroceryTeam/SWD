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
    public interface IEventService
    {
        Task<EventViewModel> GetEventById(int brandId, int productId);
        Task<BasePagingViewModel<EventViewModel>> GetEventList(int brandId, EventSearchModel searchModel, PagingRequestModel paging);
        Task<int> AddEvent(int brandId, EventCreateModel model);
        Task<bool> UpdateEvent(int eventId, EventCreateModel model);
        Task ApplyEvent(int brandId, int eventId);
        Task UnApplyEvent(int brandId, int eventId);
    }
}
