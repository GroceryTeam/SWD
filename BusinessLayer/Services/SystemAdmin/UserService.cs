using AutoMapper;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;

using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.SystemAdmin;
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
using static DataAcessLayer.Models.User;

namespace BusinessLayer.Services.SystemAdmin
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<BasePagingViewModel<UserViewModel>> GetUserList(UserSearchModel searchModel, PagingRequestModel paging)
        {
            var productsData = await _unitOfWork.UserRepository
                .Get()
                .Select
                (x => new UserViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username,
                    Email = x.Email,
                    Phone = x.Phone,
                    Status = (int)x.Status
                }
                ).ToListAsync();

            productsData = productsData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .Where(x => (searchModel.Status != null)
                                            ? x.Status == (int)searchModel.Status
                                            : true)
                        .ToList();

            int totalItem = productsData.Count;

            productsData = productsData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var productResult = new BasePagingViewModel<UserViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = productsData
            };
            return productResult;
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
                    Status = (int)x.Status
                }
                ).FirstOrDefaultAsync();
            return user;
        }
        public async Task<bool> DisableUser(int userId)
        {
            var user = await  _unitOfWork.UserRepository.Get()
                .Where(x => x.Id.Equals(userId))
                .FirstOrDefaultAsync();
            user.Status = UserStatus.Disabled;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EnableUser(int userId)
        {
            var user = await _unitOfWork.UserRepository.Get()
                .Where(x => x.Id.Equals(userId))
                .FirstOrDefaultAsync();
            user.Status = UserStatus.Enabled;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
