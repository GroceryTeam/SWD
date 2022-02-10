using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.Services;
using DataAcessLayer.Interfaces;
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
        public async Task<List<CategoryViewModel>> GetCategoryList()
        {
            var categories = await _unitOfWork.CategoryRepository
                .Get()
                .Select
                (x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }
                ).ToListAsync();
            return categories;
        }
    }
}
