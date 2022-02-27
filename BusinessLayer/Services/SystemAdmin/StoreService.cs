using AutoMapper;
using BusinessLayer.Interfaces.Notification;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.SystemAdmin;
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
namespace BusinessLayer.Services.SystemAdmin
{
    public class StoreService : BaseService, IStoreService
    {
        private readonly INotificationService _notiService;
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notiService) : base(unitOfWork, mapper)
        {
            _notiService = notiService;
        }
        public async Task<BasePagingViewModel<StoreViewModel>> GetStoreList(StoreSearchModel searchModel, PagingRequestModel paging)
        {
            var storesData = await _unitOfWork.StoreRepository
                .Get()
                .Include(x => x.Brand)
                .ThenInclude(x => x.UserBrands).ThenInclude(x => x.User)
                .Select
                (x => new StoreViewModel()
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    BrandName = x.Brand.Name,
                    Address = x.Address,
                    ApprovedStatus = (int)x.ApprovedStatus,
                    Brand = new BrandViewModel()
                    {
                        Id = x.Brand.Id,
                        Name = x.Brand.Name,
                        Status = (int)x.Brand.Status,
                        StoreList = null,
                        UserList = x.Brand.UserBrands.Select(_userbrand => new UserViewModel()
                        {
                            Id = _userbrand.User.Id,
                            Name = _userbrand.User.Name,
                            Phone = _userbrand.User.Phone,
                            Username = _userbrand.User.Username,
                            Email = _userbrand.User.Email,
                        }).ToList()
                    },
                    Name = x.Name,
                }
                )
                .ToListAsync();

            storesData = storesData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Address)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .Where(x => (searchModel.ApproveStatus != null)
                                            ? x.ApprovedStatus == (int)searchModel.ApproveStatus
                                            : true)
                         .Where(x => (searchModel.BrandId != null)
                                            ? x.BrandId == searchModel.BrandId
                                            : true)
                        .ToList();

            int totalItem = storesData.Count;

            storesData = storesData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var storeResult = new BasePagingViewModel<StoreViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = storesData
            };
            return storeResult;
        }

        public async Task<StoreViewModel> GetStoreById(int storeId)
        {
            var store = await _unitOfWork.StoreRepository
              .Get().Include(x => x.Brand)
                .ThenInclude(x => x.UserBrands).ThenInclude(x => x.User)
              .Where(x => x.Id == storeId)
.Select
                (x => new StoreViewModel()
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    BrandName = x.Brand.Name,
                    Address = x.Address,
                    ApprovedStatus = (int)x.ApprovedStatus,
                    Brand = new BrandViewModel()
                    {
                        Id = x.Brand.Id,
                        Name = x.Brand.Name,
                        Status = (int)x.Brand.Status,
                        StoreList = null,
                        UserList = x.Brand.UserBrands.Select(_userbrand => new UserViewModel()
                        {
                            Id = _userbrand.User.Id,
                            Name = _userbrand.User.Name,
                            Phone = _userbrand.User.Phone,
                            Username = _userbrand.User.Username,
                            Email = _userbrand.User.Email,
                        }).ToList()
                    },
                    Name = x.Name,
                }
                )
                .FirstOrDefaultAsync();
            if (store != null)
            {
                //var mappedStore = _mapper.Map<Store, StoreViewModel>(store);
                //mappedStore.ApprovedStatus = (int)store.ApprovedStatus;
                return store;
            }
            return null;
        }
        public async Task ChangeStoreStatus(int storeId, int status)
        {
            var store = await _unitOfWork.StoreRepository.Get().Include(x => x.Brand).Where(x => x.Id == storeId).FirstOrDefaultAsync();
            if (store != null)
            {
                store.ApprovedStatus = (Store.StoreApproveStatus)status;
                _unitOfWork.StoreRepository.Update(store);
                await _unitOfWork.SaveChangesAsync();
                if (status == (int)Store.StoreApproveStatus.Approved)
                {
                    await _notiService.SendNotificationStoreApproved(storeId, store.Brand.Id, store.Name);
                }
                else if (status == (int)Store.StoreApproveStatus.Rejected)
                {
                    await _notiService.SendNotificationStoreRejected(storeId, store.Brand.Id, store.Name);
                }
            }
        }
    }
}
