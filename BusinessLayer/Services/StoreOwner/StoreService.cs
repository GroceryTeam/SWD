using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
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
namespace BusinessLayer.Services.StoreOwner
{
    public class StoreService : BaseService, IStoreService
    {
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<List<StoreViewModel>> GetStoreList(int brandId)
        {
            var storesData = await _unitOfWork.StoreRepository
                .Get()
                .Where(x => x.BrandId == brandId)
                .ToListAsync();
            var mappedStoresData = _mapper.Map<List<Store>, List<StoreViewModel>>(storesData);
            mappedStoresData.ForEach(x => x.ApprovedStatus = (int)x.ApprovedStatus);

            return mappedStoresData;
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
        public async Task CreateStore(StoreCreateModel model)
        {
            //var mappedStore = _mapper.Map<StoreCreateModel, Store>(model);
            //mappedStore.ApprovedStatus = Store.StoreApproveStatus.Pending;
            var store = new Store()
            {
                BrandId = model.BrandId,
                Address = model.Address,
                ApprovedStatus = Store.StoreApproveStatus.Pending,
                Name = model.Name,
            };

            await _unitOfWork.StoreRepository.Add(store);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> UpdateStore(int storeId, StoreCreateModel model)
        {
            var store = await _unitOfWork.StoreRepository.Get()
                .Where(x => x.Id.Equals(storeId))
                .FirstOrDefaultAsync();
            if (store == null)
            {
                return false;
            }

            store.Name = model.Name;
            store.Address = model.Address;

            _unitOfWork.StoreRepository.Update(store);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task DeleteStore(int storeId)
        {
            var store = await _unitOfWork.StoreRepository.Get()
                .Where(x => x.Id.Equals(storeId))
                .FirstOrDefaultAsync();
            if (store == null)
            {
                throw new Exception();
            }

            store.ApprovedStatus = Store.StoreApproveStatus.Disabled;
            _unitOfWork.StoreRepository.Update(store);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
