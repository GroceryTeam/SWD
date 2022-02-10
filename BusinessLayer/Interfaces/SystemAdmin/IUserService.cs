using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.SystemAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.SystemAdmin
{
    public interface IUserService
    {
        Task<BasePagingViewModel<ResponseModels.ViewModels.SystemAdmin.UserViewModel>> GetUserList(UserSearchModel searchModel, PagingRequestModel paging);
        
        Task<ResponseModels.ViewModels.SystemAdmin.UserViewModel> GetUserById(int userId);
        Task<bool> DisableUser(int userId); 
        Task<bool> EnableUser(int userId);
    }
}
