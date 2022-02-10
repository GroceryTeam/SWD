﻿using AutoMapper;
using BusinessLayer.RequestModels.CreateModels.Cashier;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utilities
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration config
        {
            get => new MapperConfiguration(mc =>
               {
                    //view model
                    mc.CreateMap<Product, ResponseModels.ViewModels.Cashier.ProductViewModel>().ReverseMap();
                   mc.CreateMap<Product, ResponseModels.ViewModels.StoreOwner.ProductViewModel>().ReverseMap();

                   mc.CreateMap<BillDetail, ResponseModels.ViewModels.StoreOwner.BillDetailViewModel>().ReverseMap();
                   mc.CreateMap<Brand, ResponseModels.ViewModels.StoreOwner.BrandViewModel>().ReverseMap();
                   mc.CreateMap<Store, ResponseModels.ViewModels.StoreOwner.StoreViewModel>().ReverseMap();
                   mc.CreateMap<Category, ResponseModels.ViewModels.StoreOwner.CategoryViewModel>().ReverseMap();
                   mc.CreateMap<EventDetail, ResponseModels.ViewModels.StoreOwner.EventDetailViewModel>().ReverseMap(); mc.CreateMap<Event, ResponseModels.ViewModels.StoreOwner.EventDetailViewModel>().ReverseMap();
                   mc.CreateMap<Event, ResponseModels.ViewModels.StoreOwner.EventViewModel>().ReverseMap();
                   mc.CreateMap<ReceiptDetail, ResponseModels.ViewModels.StoreOwner.ReceiptDetailViewModel>().ReverseMap(); mc.CreateMap<Event, ResponseModels.ViewModels.StoreOwner.EventDetailViewModel>().ReverseMap();
                   mc.CreateMap<Receipt, ResponseModels.ViewModels.StoreOwner.ReceiptViewModel>().ReverseMap();
                   mc.CreateMap<Receipt, ResponseModels.ViewModels.StoreOwner.ReceiptViewModel>().ReverseMap();
                   mc.CreateMap<User, ResponseModels.ViewModels.StoreOwner.UserViewModel>().ReverseMap();

                   mc.CreateMap<Brand, ResponseModels.ViewModels.SystemAdmin.BrandViewModel>().ReverseMap();
                   mc.CreateMap<User, ResponseModels.ViewModels.SystemAdmin.UserViewModel>().ReverseMap();

                    //create model
                    mc.CreateMap<Bill,BillCreateModel>().ReverseMap();
                   mc.CreateMap<BillDetail, BillDetailCreateModel>().ReverseMap();
                   mc.CreateMap<Product, ProductCreateModel>().ReverseMap();
                   mc.CreateMap<User, StoreOwnerCreateModel>().ReverseMap();
               });
        }
    }
}
