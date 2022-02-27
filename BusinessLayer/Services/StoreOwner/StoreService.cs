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
                .Include(x => x.Brand)
                .ThenInclude(x => x.UserBrands).ThenInclude(x => x.User)
                .Where(x => x.BrandId == brandId)
                .Where(x => x.ApprovedStatus != Store.StoreApproveStatus.Disabled)
                .Select(x => new StoreViewModel()
                {
                    Name = x.Name,
                    Address = x.Address,
                    ApprovedStatus = (int)x.ApprovedStatus,
                    BrandId = x.BrandId,
                    BrandName = x.Brand.Name,
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
                    Id = x.Id,
                })
                .ToListAsync();
            //var mappedStoresData = _mapper.Map<List<Store>, List<StoreViewModel>>(storesData);
            //mappedStoresData.ForEach(x => x.ApprovedStatus = (int)x.ApprovedStatus);

            return storesData;
        }
        public async Task<StoreViewModel> GetStoreById(int storeId)
        {
            var store = await _unitOfWork.StoreRepository
              .Get()
              .Include(x => x.Brand)
                .ThenInclude(x => x.UserBrands).ThenInclude(x => x.User)
              .Where(x => x.Id == storeId)
               .Select(x => new StoreViewModel()
               {
                   Name = x.Name,
                   Address = x.Address,
                   ApprovedStatus = (int)x.ApprovedStatus,
                   BrandId = x.BrandId,
                   BrandName = x.Brand.Name,
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
                   Id = x.Id,
               })
              .FirstOrDefaultAsync();
            if (store != null)
            {
                //var mappedStore = _mapper.Map<Store, StoreViewModel>(store);
                //mappedStore.ApprovedStatus = (int)store.ApprovedStatus;
                return store;
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
