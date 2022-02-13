using AutoMapper;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
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

namespace BusinessLayer.Services.Cashier
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
    }
}
