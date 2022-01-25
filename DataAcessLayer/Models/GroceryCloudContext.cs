﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class GroceryCloudContext : DbContext
    {
        public GroceryCloudContext()
        {
        }

        public GroceryCloudContext(DbContextOptions<GroceryCloudContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillDetail> BillDetails { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Cashier> Cashiers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<DailyRevenue> DailyRevenues { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventDetail> EventDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBrand> UserBrands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-V4RQDKU;Initial Catalog=GroceryCloud;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.HasOne(d => d.Cashier)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.CashierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__CashierId__4222D4EF");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__StoreId__412EB0B6");
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.HasKey(e => new { e.BillId, e.ProductId })
                    .HasName("PK__BillDeta__DAB230066C38B9E9");

                entity.ToTable("BillDetail");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__BillI__44FF419A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__Produ__45F365D3");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Cashier>(entity =>
            {
                entity.ToTable("Cashier");

                entity.HasIndex(e => e.Username, "UQ__Cashier__536C85E4602BA643")
                    .IsUnique();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Cashiers)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cashier__StoreId__32E0915F");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Category__BrandI__35BCFE0A");
            });

            modelBuilder.Entity<DailyRevenue>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DailyRevenue");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Brand)
                    .WithMany()
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DailyReve__Brand__5629CD9C");

                entity.HasOne(d => d.Store)
                    .WithMany()
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DailyReve__Store__5535A963");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EventName).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__BrandId__4F7CD00D");
            });

            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.ProductId })
                    .HasName("PK__EventDet__B204047C977E6292");

                entity.ToTable("EventDetail");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDetails)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventDeta__Event__52593CB8");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.EventDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventDeta__Produ__534D60F1");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__BrandId__398D8EEE");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Categor__38996AB5");

                entity.HasOne(d => d.UnpackedProduct)
                    .WithMany(p => p.InverseUnpackedProduct)
                    .HasForeignKey(d => d.UnpackedProductId)
                    .HasConstraintName("FK__Product__Unpacke__3A81B327");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Receipt__StoreId__48CFD27E");
            });

            modelBuilder.Entity<ReceiptDetail>(entity =>
            {
                entity.HasKey(e => new { e.ReceiptId, e.ProductId })
                    .HasName("PK__ReceiptD__0748084CB09FAD79");

                entity.ToTable("ReceiptDetail");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReceiptDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptDe__Produ__4CA06362");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.ReceiptDetails)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptDe__Recei__4BAC3F29");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => new { e.StoreId, e.ProductId })
                    .HasName("PK__Stock__F0C23D6DB11FB645");

                entity.ToTable("Stock");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__ProductId__3E52440B");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__StoreId__3D5E1FD2");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Store__BrandId__2F10007B");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "UQ__User__536C85E4733884B2")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "UQ__User__5C7E359EC33A0BFE")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534B4F165C5")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<UserBrand>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BrandId })
                    .HasName("PK__UserBran__EA258349409841CB");

                entity.ToTable("UserBrand");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.UserBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBrand__Brand__2C3393D0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBrands)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBrand__UserI__2B3F6F97");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
