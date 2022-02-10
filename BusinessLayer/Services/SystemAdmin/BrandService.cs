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
using static DataAcessLayer.Models.Brand;

namespace BusinessLayer.Services.SystemAdmin
{
    public class BrandService : BaseService, IBrandService
    {
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<BasePagingViewModel<BrandViewModel>> GetBrandList(BrandSearchModel searchModel, PagingRequestModel paging)
        {
            var brandsData = await _unitOfWork.BrandRepository
                .Get().Include(x=>x.UserBrands)
                .ToListAsync();

            var brands = brandsData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .Where(x => (searchModel.Status != null)
                                            ? x.Status == (BrandStatus)searchModel.Status
                                            : true)
                        .Where(x => (searchModel.UserId != null)
                                            ? x.UserBrands.Any(x=> x.UserId == (int)searchModel.UserId)
                                            : true)
                        .Select
                                (x => new BrandViewModel()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Status = (int)x.Status
                                }
                                )
                        .ToList();

            int totalItem = brands.Count;

            brands = brands.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var brandResult = new BasePagingViewModel<BrandViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = brands
            };
            return brandResult;
        }
        public async Task<BrandViewModel> GetBrandById(int brandId)
        {
            var brand = await _unitOfWork.BrandRepository
              .Get().Where(x => x.Id == brandId)
              .Select
                (x => new BrandViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = (int)x.Status
                }
                ).FirstOrDefaultAsync();
            return brand;
        }

        public async Task<bool> DisableBrand(int brandId)
        {
            var brand = await _unitOfWork.BrandRepository.Get()
                .Where(x => x.Id.Equals(brandId))
                .FirstOrDefaultAsync();
            brand.Status = BrandStatus.Disabled;
            _unitOfWork.BrandRepository.Update(brand);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EnableBrand(int brandId)
        {
            var brand = await _unitOfWork.BrandRepository.Get()
                .Where(x => x.Id.Equals(brandId))
                .FirstOrDefaultAsync();
            brand.Status = BrandStatus.Disabled;
            _unitOfWork.BrandRepository.Update(brand);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
