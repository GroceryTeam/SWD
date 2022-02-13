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
using AutoMapper;

namespace BusinessLayer.Services.StoreOwner
{
    public class EventService : BaseService, IEventService
    {
        public EventService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<EventViewModel> GetEventById(int brandId, int eventId)
        {
            // var eventDetails = await _unitOfWork.EventDetailRepository.Get().Where(x => x.EventId == eventId).ToListAsync();
            var _event = await _unitOfWork.EventRepository
              .Get()
              .Where(x => x.BrandId == brandId)
              .Where(x => x.Id == eventId)
               .Include(x => x.EventDetails)
              .Select
                (x => new EventViewModel()
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
                }).FirstOrDefaultAsync<EventViewModel>();
            return _event;
        }

        public async Task<BasePagingViewModel<EventViewModel>> GetEventList(int brandId, EventSearchModel searchModel, PagingRequestModel paging)
        {
            var eventData = await _unitOfWork.EventRepository
                .Get().Where(x => x.BrandId == brandId)
                .Select
                (x => new EventViewModel()
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

            var eventResult = new BasePagingViewModel<EventViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = eventData
            };
            return eventResult;
        }

        public async Task<int> AddEvent(int brandId, EventCreateModel model)
        {
            var mappedEvent = _mapper.Map<EventCreateModel, Event>(model);
            mappedEvent.Status = EventStatus.Disabled;
            await _unitOfWork.EventRepository.Add(mappedEvent);

            foreach (var detail in model.Details)
            {
                mappedEvent.EventDetails.Add(new EventDetail
                {
                    EventId = mappedEvent.Id,
                    NewPrice = detail.NewPrice,
                    ProductId = detail.ProductId
                });
            }
            await _unitOfWork.SaveChangesAsync();

            return mappedEvent.Id;

        }
        public async Task<bool> UpdateEvent(int eventId, EventCreateModel model)
        {
            var @event = await _unitOfWork.EventRepository.Get()
                .Where(x => x.Id.Equals(eventId))
                .FirstOrDefaultAsync();
            if (@event == null)
            {
                return false;
            }

            @event.EventName = model.EventName;

            var oldEventDetailIdList = _unitOfWork.EventDetailRepository.Get().Where(x => x.EventId == eventId).Select(x => new { x.EventId, x.ProductId }).ToList();
            foreach (var _id in oldEventDetailIdList)
            {
                await _unitOfWork.EventDetailRepository.DeleteComplex(_id.EventId, _id.ProductId);
            }
            @event.EventDetails.Clear();
            foreach (var detail in model.Details)
            {
                @event.EventDetails.Add(new EventDetail
                {
                    EventId = @event.Id,
                    NewPrice = detail.NewPrice,
                    ProductId = detail.ProductId
                });
            }

            _unitOfWork.EventRepository.Update(@event);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task ApplyEvent(int brandId, int eventId)
        {
            var appliedEvent = await _unitOfWork.EventRepository.Get()
                .Where(x => x.Status == EventStatus.Enabled)
                .Where(x => x.BrandId == brandId)
                .FirstOrDefaultAsync();
            if (appliedEvent != null) appliedEvent.Status = EventStatus.Disabled;
            var newAppliedEvent = await _unitOfWork.EventRepository.Get()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();
            newAppliedEvent.Status = EventStatus.Enabled;
            _unitOfWork.EventRepository.Update(appliedEvent);
            _unitOfWork.EventRepository.Update(newAppliedEvent);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UnApplyEvent(int brandId, int eventId)
        {
            var appliedEvent = await _unitOfWork.EventRepository.Get()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();
            appliedEvent.Status = EventStatus.Disabled;
            _unitOfWork.EventRepository.Update(appliedEvent);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteEvent(int eventId)
        {
           await _unitOfWork.EventRepository.Delete(eventId);
        }
    }
}
