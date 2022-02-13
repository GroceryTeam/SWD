using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.Services;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.StoreOwner
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        //viet ham cho service
        public async Task<BasePagingViewModel<CategoryViewModel>> GetCategoryList(int brandId, PagingRequestModel paging)
        {
            var categoryData = await _unitOfWork.CategoryRepository
                .Get().Where(x => x.BrandId == brandId).ToListAsync();

            var mappedCategoryData = _mapper.Map<List<Category>, List<CategoryViewModel>>(categoryData);
            int totalItem = mappedCategoryData.Count;

            mappedCategoryData = mappedCategoryData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var categoryResult = new BasePagingViewModel<CategoryViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = mappedCategoryData
            };
            return categoryResult;
        }

        public async Task<int> AddCategory(int brandId, CategoryCreateModel model)
        {
            var mappedCategory = _mapper.Map<CategoryCreateModel, Category>(model);
            await _unitOfWork.CategoryRepository.Add(mappedCategory);
            await _unitOfWork.SaveChangesAsync();
            return mappedCategory.Id;
        }

        public async Task<bool> UpdateCategory(int brandId, int categoryId, string CategoryName)
        {
            var category = await _unitOfWork.CategoryRepository.Get()
                .Where(x => x.BrandId.Equals(brandId))
                .Where(x => x.Id.Equals(categoryId))
                .FirstOrDefaultAsync();
            if (category == null)
            {
                return false;
            }

            category.Name = CategoryName;

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
