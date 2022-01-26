﻿using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Bill> BillRepository { get; }
        IGenericRepository<BillDetail> BillDetailRepository { get; }
        IGenericRepository<Brand> BrandRepository { get; }
        IGenericRepository<Cashier> CashierRepository { get; }
        IGenericRepository<DailyRevenue> DailyrevenueRepository { get; }
        IGenericRepository<Event> EventRepository { get; }
        IGenericRepository<EventDetail> EventDetailRepository { get; }
        IGenericRepository<Receipt> ReceiptRepository { get; }
        IGenericRepository<ReceiptDetail> ReceiptDetailRepository { get; }
        IGenericRepository<Stock> StockRepository { get; }
        IGenericRepository<Store> StoreRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        GroceryCloudContext Context();

        Task SaveChangesAsync();
    }
}
