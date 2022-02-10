using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.Cashier;
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
using static DataAcessLayer.Models.Cashier;

namespace BusinessLayer.Services.StoreOwner
{
    public class CashierSevice : BaseService, ICashierService
    {
        public CashierSevice(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        public async Task<CashierViewModel> Login(LoginModel login)
        {
            var cashier = await _unitOfWork.CashierRepository
                .Get().Where(x => x.Username == login.Username && x.Password == login.Password)
                .Where(x => x.Status == CashierStatus.Working)
                .Select(x => new CashierViewModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Name = x.Name,
                    StoreId = x.StoreId
                }).FirstOrDefaultAsync();
            return cashier;
        }
    }
}
