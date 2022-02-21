using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.Services;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using static DataAcessLayer.Models.Event;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using AutoMapper;
using DataAcessLayer.Interfaces;

namespace BusinessLayer.Services.Cashier
{
    public class EventService : BaseService, Interfaces.Cashier.IEventService
    {
        public EventService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<EventViewModel> GetCurrentAppliedEvent(int brandId)
        {
            // var eventDetails = await _unitOfWork.EventDetailRepository.Get().Where(x => x.EventId == eventId).ToListAsync();
            var _event = await _unitOfWork.EventRepository
              .Get()
              .Where(x => x.BrandId == brandId)
               .Where(x => x.Status ==  EventStatus.Enabled)
               .Include(x => x.EventDetails)
              .Select
                (x => new EventViewModel()
                {
                    Id = x.Id,
                    EventName = x.EventName,
                    Status = (int)x.Status,
                    ProductCount = x.EventDetails.Count,
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
    }
}
