using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.Services;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using static DataAcessLayer.Models.Event;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;

namespace BusinessLayer.Services.StoreOwner
{
    public class EventService : BaseService, IEventService
    {
        public EventService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<EventsViewModel> GetEventById(int brandId, int eventId)
        {
            // var eventDetails = await _unitOfWork.EventDetailRepository.Get().Where(x => x.EventId == eventId).ToListAsync();
            var _event = await _unitOfWork.EventRepository
              .Get()
              .Where(x => x.BrandId == brandId)
              .Where(x => x.Id == eventId)
               .Include(x => x.EventDetails)
              .Select
                (x => new EventsViewModel()
                {
                    Id = x.Id,
                    EventName = x.EventName,
                    Status = (int)x.Status,
                    EventDetails =
                       (List<EventDetailViewModel>)x.EventDetails
                    .Select
                        (x => new EventDetailViewModel()
                        {
                            EventId = x.EventId,
                            NewPrice = x.NewPrice,
                            OriginalPrice = x.Product.SellPrice,
                            ProductId = x.ProductId,
                            ProductName = x.Product.Name
                        }
                        )
                }).FirstOrDefaultAsync<EventsViewModel>();
            return _event;
        }

        public async Task<BasePagingViewModel<EventsViewModel>> GetEventList(int brandId, EventSearchModel searchModel, PagingRequestModel paging)
        {
            var eventData = await _unitOfWork.EventRepository
                .Get().Where(x => x.BrandId == brandId)
                .Select
                (x => new EventsViewModel()
                {
                    Id = x.Id,
                    EventName = x.EventName,
                    Status = (int)x.Status
                }
                ).ToListAsync();

            eventData = eventData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.EventName)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .Where(x => (searchModel.Status != null)
                                            ? x.Status == (int)searchModel.Status
                                            : true)
                        .ToList();

            int totalItem = eventData.Count;

            eventData = eventData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var eventResult = new BasePagingViewModel<EventsViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = eventData
            };
            return eventResult;
        }
    }
}
