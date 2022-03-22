using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
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
        private readonly FirebaseApp _firebaseApp;
        private readonly FirebaseAuth _firebaseAuth;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            FirebaseApp firebaseApp) : base(unitOfWork, mapper)
        {
            _firebaseApp = firebaseApp;
            _firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
        }
        public async Task<UserViewModel> GetInformation(int userId)
        {
            var storeowner = await _unitOfWork.UserRepository
              .Get()
             .Where(x => x.Id == userId)
              .Select(x => new UserViewModel()
              {
                  Id = x.Id,
                  Username = x.Username,
                  Name = x.Name,
                  Email = x.Email,
                  Phone = x.Phone
              }).FirstOrDefaultAsync();
            return storeowner;
        }
        public async Task<UserViewModel> GetUserById(int userId)
        {
            var user = await _unitOfWork.UserRepository
              .Get().Where(x => x.Id == userId)
              .Select
                (x => new UserViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username,
                    Email = x.Email,
                    Phone = x.Phone,
                }
                ).FirstOrDefaultAsync();
            return user;
        }
        public async Task<List<UserViewModel> > GetUserByPhoneNoOrEmailOrUsername(string searchTerm)
        {
            var userList = await _unitOfWork.UserRepository
              .Get()
              .Select
                (x => new UserViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username,
                    Email = x.Email,
                    Phone = x.Phone,
                }
                ).ToListAsync();
            userList = userList.Where(x => x.Username.Contains(searchTerm)
            || x.Email.Contains(searchTerm)
             || x.Phone.Contains(searchTerm)
            ).ToList();
            return userList;
        }
        public async Task<UserViewModel> Login(LoginModel login)
        {
            var storeowner = await _unitOfWork.UserRepository
                .Get()
                .Where(x => x.Username == login.Username && x.Password == login.Password)
                .Where(x => x.Status == UserStatus.Enabled)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone
                }).FirstOrDefaultAsync();
            return storeowner;
        }
        public async Task<UserViewModel> LoginFirebase(LoginFirebaseModel login)
        {
            var decodedToken = await _firebaseAuth
                    .VerifyIdTokenAsync(login.IdToken);
            string uid = decodedToken.Uid;
            var userInfo = await _firebaseAuth.GetUserAsync(uid);

            var user = await _unitOfWork.UserRepository
                .Get()
                .Where(x => x.Email == userInfo.Email)
                .Where(x => x.Status == UserStatus.Enabled)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone
                }).FirstOrDefaultAsync();
            return user;
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
