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
        Task<BasePagingViewModel<UserViewModel>> GetUserList(UserSearchModel searchModel, PagingRequestModel paging)
        Task<StoreOwnerViewModel> Login(LoginModel login);
        Task<SignupErrorModel> Signup(StoreOwnerCreateModel model);
    }
}
