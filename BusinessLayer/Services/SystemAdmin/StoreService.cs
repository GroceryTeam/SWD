using AutoMapper;
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
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<BasePagingViewModel<StoreViewModel>> GetStoreList(StoreSearchModel searchModel, PagingRequestModel paging)
        {
            var storesData = await _unitOfWork.StoreRepository
                .Get()
                .Where(x => x.BrandId == searchModel.BrandId)
                .Select
                (x => new StoreViewModel()
                {
                      Id = x.Id,
                      BrandId = x.BrandId,
                      Address = x.Address,
                      ApprovedStatus = (int)x.ApprovedStatus,
                      Name = x.Name
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
              .Get()
              .Where(x => x.Id == storeId)
              .FirstOrDefaultAsync();
            if (store != null)
            {
                var mappedStore = _mapper.Map<Store, StoreViewModel>(store);
                mappedStore.ApprovedStatus = (int)store.ApprovedStatus;
                return mappedStore;
            }
            return null;
        }
        public async Task ChangeStoreStatus(int storeId, int status)
        {
            var store = await _unitOfWork.StoreRepository.Get().Where(x => x.Id == storeId).FirstOrDefaultAsync();
            if (store!=null)
            {
                store.ApprovedStatus = (Store.StoreApproveStatus)status;
                _unitOfWork.StoreRepository.Update(store);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
