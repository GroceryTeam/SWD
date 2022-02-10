using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
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
namespace BusinessLayer.Services.StoreOwner
{
    public class StoreService : BaseService, IStoreService
    {
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<List<StoreViewModel>> GetStoreList(int brandId)
        {
            var storesData = await _unitOfWork.StoreRepository
                .Get()
                .Where(x => x.BrandId == brandId)
                .ToListAsync();
            var mappedStoresData = _mapper.Map<List<Store>, List<StoreViewModel>>(storesData);
            mappedStoresData.ForEach(x => x.ApprovedStatus = (int)x.ApprovedStatus);

            return mappedStoresData;
        }
    }
}
