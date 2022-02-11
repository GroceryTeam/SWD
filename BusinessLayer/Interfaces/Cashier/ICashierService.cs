using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.ResponseModels.ViewModels.Cashier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.Cashier
{
    public interface ICashierService
    {
        Task<CashierViewModel> Login(LoginModel login);
    }
}
