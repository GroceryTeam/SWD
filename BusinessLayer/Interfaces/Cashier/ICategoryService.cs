using BusinessLayer.RequestModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.Cashier
{
    public interface ICategoryService
    {
        Task<BasePagingViewModel<CategoryViewModel>> GetCategoryList(int brandId, PagingRequestModel paging);
    }
}
