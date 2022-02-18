using AutoMapper;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
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
using static DataAcessLayer.Models.Cashier;

namespace BusinessLayer.Services.StoreOwner
{
    public class CashierSevice : BaseService, ICashierService
    {
        public CashierSevice(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        public async Task<BasePagingViewModel<CashierViewModel>> GetCashierList(CashierSearchModel searchModel, PagingRequestModel paging)
        {
            var cashiersData = await _unitOfWork.CashierRepository
                .Get()
                .Include(x => x.Store).ThenInclude(x => x.Brand)
                .Where(x => searchModel.BrandId != null ? x.Store.Brand.Id == searchModel.BrandId : true)
                .Where(x => searchModel.StoreId != null ? x.Store.Id == searchModel.StoreId : true)
                .Where(x => searchModel.IncludeDisabledCashier ? true : x.Status == CashierStatus.Working)
                .ToListAsync();
            var mappedCashiersData = _mapper.Map<List<DataAcessLayer.Models.Cashier>, List<CashierViewModel>>(cashiersData);
            mappedCashiersData.ForEach(x => x.Status = (int)x.Status);

            mappedCashiersData = mappedCashiersData
                        .Where(x =>
                            StringNormalizer.VietnameseNormalize(x.Name)
                            .Contains(StringNormalizer.VietnameseNormalize(searchModel.SearchTerm)))
                        .ToList();

            int totalItem = mappedCashiersData.Count;

            mappedCashiersData = mappedCashiersData.Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize).ToList();

            var cashierResult = new BasePagingViewModel<CashierViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = mappedCashiersData
            };
            return cashierResult;
        }
        public async Task<CashierViewModel> GetCashierById(int cashierId)
        {
            var cashier = await _unitOfWork.CashierRepository
              .Get()
              .Where(x => x.Id == cashierId)
              .FirstOrDefaultAsync();
           
            if (cashier != null)
            {
                var mappedCashier = _mapper.Map<DataAcessLayer.Models.Cashier, CashierViewModel>(cashier);
                mappedCashier.Status = (int)cashier.Status;
                return mappedCashier;
            }
            return null;
        }
        public async Task<int> AddCashier(int storeId, CashierCreateModel model)
        {

            var cashier = new DataAcessLayer.Models.Cashier()
            {
                Name = model.Name,
                StoreId = model.StoreId,
                Username = model.Username,
                Status = CashierStatus.Working,
                Password = model.Password,
            };
            //var mappedProduct = _mapper.Map<ProductCreateModel, Product>(model);
            //mappedProduct.BrandId = brandId;
            //mappedProduct.Status = Product.ProductStatus.Selling;

            await _unitOfWork.CashierRepository.Add(cashier);
            await _unitOfWork.SaveChangesAsync();

            return cashier.Id;
        }
        public async Task<bool> UpdateCashier(int cashierId, CashierCreateModel model)
        {
            var cashier = await _unitOfWork.CashierRepository.Get()
                .Where(x => x.Id.Equals(cashierId))
                .FirstOrDefaultAsync();
            if (cashier == null)
            {
                return false;
            }

            cashier.Name = model.Name;
            cashier.Password = model.Password;

            _unitOfWork.CashierRepository.Update(cashier);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task DeleteCashier(int cashierId)
        {
            var cashier = await _unitOfWork.CashierRepository.Get()
                .Where(x => x.Id.Equals(cashierId))
                .FirstOrDefaultAsync();

            _unitOfWork.CashierRepository.Update(cashier);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
