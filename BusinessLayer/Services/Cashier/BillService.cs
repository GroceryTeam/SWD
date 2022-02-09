using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.Cashier;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
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
namespace BusinessLayer.Services.Cashier
{
    public class BillService : BaseService, IBillService
    {
        public BillService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        //public async Task<int> AddBill(int storeId,int cashierId,BillCreateModel model)
        //{


        //}
    }
}
