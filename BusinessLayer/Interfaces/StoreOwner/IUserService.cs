using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.StoreOwner
{
    public interface IUserService
    {
        Task<UserViewModel> Login(LoginModel login);
        Task<SignupErrorModel> Signup(StoreOwnerCreateModel model);
        Task<UserViewModel> LoginFirebase(LoginFirebaseModel login);
    }
}
