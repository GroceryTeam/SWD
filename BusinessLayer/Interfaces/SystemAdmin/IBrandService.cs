using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.SystemAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.SystemAdmin
{
    public interface IBrandService
    {
        Task<BasePagingViewModel<BrandViewModel>> GetBrandList(BrandSearchModel searchModel, PagingRequestModel paging);
        Task<BrandViewModel> GetBrandById(int brandId);
        Task<bool> DisableBrand(int brandId);
        Task<bool> EnableBrand(int brandId);
    }
}
