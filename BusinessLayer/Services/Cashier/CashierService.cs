using AutoMapper;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.ResponseModels.ViewModels.Cashier;
using DataAcessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Cashier;

namespace BusinessLayer.Services.Cashier
{
     public class CashierService: BaseService, ICashierService
     {
        public CashierService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        public async Task<CashierViewModel> Login(LoginModel login)
        {
            var cashier = await _unitOfWork.CashierRepository
                .Get().Include(x => x.Store).ThenInclude(x => x.Brand).Where(x => x.Username == login.Username && x.Password == login.Password)
                .Where(x => x.Status == CashierStatus.Working)
                .Select(x => new CashierViewModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    BrandId = x.Store.Brand.Id,
                    BrandName = x.Store.Brand.Name,
                    Name = x.Name,
                    StoreId = x.StoreId
                }).FirstOrDefaultAsync();
            return cashier;
        }
    }
}
