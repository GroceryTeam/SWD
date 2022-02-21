using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.Cashier;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ErrorModels.Cashier;
using BusinessLayer.ResponseModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.Cashier
{
    public interface IBillService
    {
        Task<AddBillErrorModel> AddBill(int storeId, int cashierId, BillCreateModel model);
    }
}
