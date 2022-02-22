using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.Cashier;
using BusinessLayer.ResponseModels.ViewModels.Cashier;
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
        Task<BasePagingViewModel<ProductViewModel>> GetProductList(int brandId, ProductSearchModel searchModel, PagingRequestModel paging);
        Task<ProductViewModel> GetProductById(int storeId, int productId);
        Task UnpackProduct(int productId, int number, int storeId);
    }
}
