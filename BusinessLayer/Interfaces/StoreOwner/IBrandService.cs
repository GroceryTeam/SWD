using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.StoreOwner
{
    public interface IBrandService
    {
        Task<List<BrandViewModel>> GetBrandList(int userId);
        Task<bool> AddUserToBrand(string phoneNo, string email, int brandId);
        Task<int> AddBrand(BrandCreateModel model);

    }
}
