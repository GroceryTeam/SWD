using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
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
namespace BusinessLayer.Services.StoreOwner
{
    public class BrandService : BaseService, IBrandService
    {
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<List<BrandViewModel>> GetBrandList(int userId)
        {
            var userBrandData = await _unitOfWork.UserBrandRepository
                .Get()
                .Where(x => x.UserId == userId)
                .Include(x => x.Brand)
                .ThenInclude(x => x.Stores)
                .Select(x => x.Brand)
                .Select
                                (x => new BrandViewModel()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Status = (int)x.Status,
                                    UserIdList = x.UserBrands.Select(x => x.UserId).ToList(),
                                    StoreList = x.Stores.Select(store => new StoreViewModel()
                                    {
                                        Id = store.Id,
                                        Address = store.Address,
                                        ApprovedStatus = (int)store.ApprovedStatus,
                                        BrandId = store.BrandId,
                                        Name = store.Name,
                                    }
                                    ).ToList()
                                }
                                )
                .ToListAsync();
            //var mappedBrandsData = _mapper.Map<List<Brand>, List<BrandViewModel>>(userBrandData);
            //mappedBrandsData.ForEach(x => x.Status = (int)x.Status);

            return userBrandData;
        }
        public async Task<bool> AddUserToBrand(string phoneNo, string email, int brandId)
        {
            var user = await _unitOfWork.UserRepository.Get()
                .Where(x => (phoneNo != null) ? x.Phone.Contains(phoneNo.Trim()) : true)
                .Where(x => (email != null) ? x.Email.Contains(email.Trim()) : true)
                .FirstOrDefaultAsync();
            if (user == null) return false;
            else
            {
                if (_unitOfWork.UserBrandRepository.Get().Where(x => x.BrandId == brandId && x.UserId == user.Id).FirstOrDefault() != null)
                {
                    var userBrand = new UserBrand()
                    {
                        BrandId = brandId,
                        UserId = user.Id
                    };
                    await _unitOfWork.UserBrandRepository.Add(userBrand);
                    await _unitOfWork.SaveChangesAsync();
                }
                return true;
            }
        }
        public async Task DeleteUserFromBrand(int userId, int brandId)
        {
            var userBrand = await _unitOfWork.UserBrandRepository.Get()
                .Where(x => x.UserId == userId)
                .Where(x => x.BrandId == brandId)
                .FirstOrDefaultAsync();
            await _unitOfWork.UserBrandRepository.DeleteComplex(userBrand.UserId, userBrand.BrandId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> AddBrand(BrandCreateModel model)
        {
            var brand = new Brand()
            {
                Name = model.Name,
                Status = Brand.BrandStatus.Enabled,
            };
            await _unitOfWork.BrandRepository.Add(brand);
            await _unitOfWork.SaveChangesAsync();
            var userBrand = new UserBrand()
            {
                BrandId = brand.Id,
                UserId = model.UserId
            };
            await _unitOfWork.UserBrandRepository.Add(userBrand);
            await _unitOfWork.SaveChangesAsync();
            return brand.Id;
        }
        public async Task DisableBrand(int brandId)
        {
            var brand = await _unitOfWork.BrandRepository.Get()
                .Where(x => x.Id.Equals(brandId))
                .FirstOrDefaultAsync();

            brand.Status = Brand.BrandStatus.Disabled;
            _unitOfWork.BrandRepository.Update(brand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
