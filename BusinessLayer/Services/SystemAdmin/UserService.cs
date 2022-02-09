using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ErrorModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using BusinessLayer.Services;
using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static BusinessLayer.ResponseModels.ErrorModels.StoreOwner.SignupErrorModel;
using static DataAcessLayer.Models.User;

namespace BusinessLayer.Services.SystemAdmin
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        //public async Task<BasePagingViewModel<Us>> GetUserList(int brandId, ProductSearchModel searchModel, PagingRequestModel paging)
        //{
        //    var productsData = await _unitOfWork.ProductRepository
        //        .Get().Where(x => x.BrandId == brandId)
        //        .Select
        //        (x => new ProductsViewModel()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            UnpackedProductId = x.UnpackedProductId,
        //            UnpackedProductName = x.UnpackedProduct.Name,
        //            BuyPrice = x.BuyPrice,
        //            SellPrice = x.SellPrice,
        //            CategoryId = x.CategoryId,
        //            CategoryName = x.Category.Name,
        //            ConversionRate = x.ConversionRate,
        //            UnitLabel = x.UnitLabel,
        //            LowerThreshold = x.LowerThreshold,
        //            Status = (int)x.Status
        //        }
        //        ).ToListAsync();

        //    productsData = productsData
        //                .Where(x =>
        //                    StringNormalizer.VietnameseNormalize(x.Name)
        //                    .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
        //                .Where(x => (searchModel.MinimumBuyingPrice != null)
        //                                    ? x.BuyPrice >= searchModel.MinimumBuyingPrice
        //                                    : true)
        //                .Where(x => (searchModel.MaximumBuyingPrice != null)
        //                                    ? x.BuyPrice <= searchModel.MaximumBuyingPrice
        //                                    : true)
        //                .Where(x => (searchModel.MinimumSellingPrice != null)
        //                                    ? x.SellPrice >= searchModel.MinimumSellingPrice
        //                                    : true)
        //                .Where(x => (searchModel.MaximumSellingPrice != null)
        //                                    ? x.SellPrice <= searchModel.MaximumSellingPrice
        //                                    : true)
        //                .Where(x => (searchModel.Status != null)
        //                                    ? x.Status == (int)searchModel.Status
        //                                    : true)
        //                .ToList();

        //    int totalItem = productsData.Count;

        //    productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
        //        .Take(paging.PageSize).ToList();

        //    var productResult = new BasePagingViewModel<ProductsViewModel>()
        //    {
        //        PageIndex = paging.PageIndex,
        //        PageSize = paging.PageSize,
        //        TotalItem = totalItem,
        //        TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
        //        Data = productsData
        //    };
        //    return productResult;
        //}
        public async Task<StoreOwnerViewModel> Login(LoginModel login)
        {
            var cashier = await _unitOfWork.UserRepository 
                .Get().Where(x => x.Username == login.Username && x.Password == login.Password)
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
            if (tempUser!=null)
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
