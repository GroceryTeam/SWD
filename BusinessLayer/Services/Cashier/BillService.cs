using AutoMapper;
using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.Cashier;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.ResponseModels.ErrorModels.Cashier;
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
        private readonly IProductService _productService;
        public BillService(IUnitOfWork unitOfWork, IMapper mapper, IProductService productService) : base(unitOfWork, mapper)
        {
            _productService = productService;
        }
        public async Task<AddBillErrorModel> AddBill(int storeId, int cashierId, BillCreateModel model)
        {
            //check so luong va tra loi
            var error = new AddBillErrorModel()
            {
                ErrorProducts = new List<ProductErrorModel>()
            };
            foreach (var _detail in model.Details)
            {
                var productInDetail = _unitOfWork.ProductRepository
                       .Get()
                       .Where(x => x.Id == _detail.ProductId)
                       .Include(x => x.Stocks)
                       .FirstOrDefault();
                var correspondingStocks = productInDetail.Stocks
                   .Where(x => x.StoreId == storeId)
                   .Where(x => x.ProductId == productInDetail.Id)
                   .ToList();
                int quantityInStock = 0;
                foreach (var stock in correspondingStocks)
                {
                    quantityInStock += stock.Quantity;
                }
                if (_detail.Quantity > quantityInStock)
                {
                    error.ErrorProducts.Add(new ProductErrorModel()
                    {
                        Id = _detail.ProductId,
                        Name = productInDetail.Name
                       ,
                        RemainingQuantity = quantityInStock
                    });
                }
            }
            if (error.ErrorProducts.Count() == 0)
            {
                var bill = new Bill()
                {
                    TotalPrice = model.TotalPrice,
                    CashierId = cashierId,
                    DateCreated = DateTime.Now,
                    StoreId = storeId,
                };
                await _unitOfWork.BillRepository.Add(bill);
                await _unitOfWork.SaveChangesAsync();
                foreach (var detail in model.Details)
                {
                    var remainingQuantity = detail.Quantity;
                    var productInDetail = _unitOfWork.ProductRepository
                        .Get()
                        .Where(x => x.Id == detail.ProductId)
                        .Include(x => x.Stocks)
                        .FirstOrDefault();
                    var correspondingStocks = productInDetail.Stocks
                       .Where(x => x.StoreId == storeId)
                       .Where(x => x.ProductId == productInDetail.Id)
                       .ToList();
                    //de lay currentPrice
                    var productViewModel = await _productService.GetProductById(storeId, detail.ProductId);

                    do

                    {
                        //tim cai sellinng
                        var sellingStock = correspondingStocks.Where(x => x.Status == Stock.StockDetail.Selling).FirstOrDefault();
                        int quantityInThisBillDetail = Math.Min(sellingStock.Quantity, remainingQuantity);
                        //tru ra 

                        remainingQuantity -= quantityInThisBillDetail;
                        sellingStock.Quantity -= quantityInThisBillDetail;
                        _unitOfWork.StockRepository.Update(sellingStock);
                        //va ghi vao bill
                        bill.BillDetails.Add(new BillDetail
                        {
                            ProductId = detail.ProductId,
                            BillId = bill.Id,
                            BuyPrice = sellingStock.BuyPrice,
                            SellPrice = productViewModel.EventPrice,
                            Quantity = quantityInThisBillDetail,
                            StockId = sellingStock.Id
                        }); ;
                        //xem thu het hang thi next thanh selling
                        if (sellingStock.Quantity == 0)
                        {
                            sellingStock.Status = Stock.StockDetail.SoldOut;
                            var availableStock = correspondingStocks
                                   .Where(x => x.Status == Stock.StockDetail.Available);
                            var nextStockId = availableStock.Min(x => x.Id);
                            var nextSellingStock = correspondingStocks.Where(x => x.Id == nextStockId).FirstOrDefault();
                            if (nextSellingStock != null)
                            {
                                nextSellingStock.Status = Stock.StockDetail.Selling;
                                _unitOfWork.StockRepository.Update(nextSellingStock);
                            }

                        }
                    } while (remainingQuantity > 0);
                }

                _unitOfWork.BillRepository.Update(bill);
                await _unitOfWork.SaveChangesAsync();
            }

            return error;

        }


    }
}
