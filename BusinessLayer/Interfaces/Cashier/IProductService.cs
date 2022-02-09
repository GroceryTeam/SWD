using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.Cashier;
using BusinessLayer.ResponseModel.ViewModels.Cashier;
using BusinessLayer.ResponseModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.Cashier
{
    public interface IProductService
    {
        Task<BasePagingViewModel<ProductsViewModel>> GetProductList(int brandId, ProductSearchModel searchModel, PagingRequestModel paging);
    }
}
