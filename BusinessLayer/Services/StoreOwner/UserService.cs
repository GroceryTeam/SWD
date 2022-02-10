using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.ResponseModels.ErrorModels.StoreOwner.SignupErrorModel;
using static DataAcessLayer.Models.User;

namespace BusinessLayer.Services.StoreOwner
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<StoreOwnerViewModel> Login(LoginModel login)
        {
            var cashier = await _unitOfWork.UserRepository
                .Get()
                .Where(x => x.Username == login.Username && x.Password == login.Password)
                .Where(x => x.Status == UserStatus.Enabled)
                .Select(x => new StoreOwnerViewModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone
                }).FirstOrDefaultAsync();
            return cashier;
        }
        public async Task<SignupErrorModel> Signup(StoreOwnerCreateModel model)
        {
            SignupErrorModel error = new SignupErrorModel();
            var tempUser = await _unitOfWork.UserRepository
               .Get().Where(x => x.Username == model.Username).FirstOrDefaultAsync();
            if (tempUser != null)
            {
                error.Error = SignupError.UsernameExists;
                return error;
            }
            tempUser = await _unitOfWork.UserRepository
              .Get().Where(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (tempUser != null)
            {
                error.Error = SignupError.EmailExists;
                return error;
            }
            tempUser = await _unitOfWork.UserRepository
              .Get().Where(x => x.Phone == model.Phone).FirstOrDefaultAsync();
            if (tempUser != null)
            {
                error.Error = SignupError.PhoneNumberExists;
                return error;
            }
            var newUser = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,
                Username = model.Username,
                Status = User.UserStatus.Enabled
            };
            await _unitOfWork.UserRepository.Add(newUser);
            await _unitOfWork.SaveChangesAsync();
            return null;
        }
    }
}
