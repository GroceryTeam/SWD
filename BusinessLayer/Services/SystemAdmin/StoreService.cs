﻿using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.Services;
using BusinessLayer.ViewModels;
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
namespace BusinessLayer.Services.SystemAdmin
{
    public class StoreService : BaseService, IStoreService
    {
        public StoreService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
