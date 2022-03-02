using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GroceryCloud18th2Context _dbContext;



        public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<Category> CategoryRepository { get; }
        public IGenericRepository<Bill> BillRepository { get; }
        public IGenericRepository<BillDetail> BillDetailRepository { get; }
        public IGenericRepository<UserBrand> UserBrandRepository { get; }
        public IGenericRepository<Brand> BrandRepository { get; }
        public IGenericRepository<Cashier> CashierRepository { get; }
        public IGenericRepository<DailyRevenue> DailyRevenueRepository { get; }
        public IGenericRepository<Event> EventRepository { get; }
        public IGenericRepository<EventDetail> EventDetailRepository { get; }
        public IGenericRepository<Receipt> ReceiptRepository { get; }
        public IGenericRepository<ReceiptDetail> ReceiptDetailRepository { get; }
        public IGenericRepository<Stock> StockRepository { get; }
        public IGenericRepository<Store> StoreRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<FcmtokenMobile> FcmTokenMobileRepository { get; }
        public UnitOfWork(GroceryCloud18th2Context dbContext,
            IGenericRepository<Product> productRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<Bill> billRepository,
            IGenericRepository<BillDetail> billDetailRepository,
            IGenericRepository<Brand> brandRepository,
            IGenericRepository<Cashier> cashierRepository,
            IGenericRepository<DailyRevenue> dailyrevenueRepository,
            IGenericRepository<Event> eventRepository,
            IGenericRepository<EventDetail> eventDetailRepository,
            IGenericRepository<Receipt> receiptRepository,
            IGenericRepository<ReceiptDetail> receiptDetailRepository,
            IGenericRepository<Stock> stockRepository,
            IGenericRepository<Store> storeRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<UserBrand> userBrandRepository,
            IGenericRepository<FcmtokenMobile> fcmTokenMobileRepository
            )
        {
            _dbContext = dbContext;

            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            BillRepository = billRepository;
            BillDetailRepository = billDetailRepository;
            BrandRepository = brandRepository;
            CashierRepository = cashierRepository;
            DailyRevenueRepository = dailyrevenueRepository;
            EventRepository = eventRepository;
            EventDetailRepository = eventDetailRepository;
            ReceiptRepository = receiptRepository;
            ReceiptDetailRepository = receiptDetailRepository;
            StockRepository = stockRepository;
            StoreRepository = storeRepository;
            UserRepository = userRepository;
            UserBrandRepository = userBrandRepository;
            FcmTokenMobileRepository = fcmTokenMobileRepository;
        }

        public GroceryCloud18th2Context Context()
        {
            return _dbContext;
        }

        //public DatabaseFacade Database()
        //{
        //    return _dbContext.Database;
        //}

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
