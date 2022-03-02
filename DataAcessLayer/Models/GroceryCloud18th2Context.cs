using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class GroceryCloud18th2Context : DbContext
    {
        public GroceryCloud18th2Context()
        {
        }

        public GroceryCloud18th2Context(DbContextOptions<GroceryCloud18th2Context> options)
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
        public virtual DbSet<FcmtokenMobile> FcmtokenMobiles { get; set; }
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
                optionsBuilder.UseSqlServer("Data Source=grocerycloudsqlserver.database.windows.net;Initial Catalog=GroceryCloud18th2;User ID=groceryAdmin;Password=SWD@123123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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
                    .HasConstraintName("FK__Bill__CashierId__02084FDA");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__StoreId__01142BA1");
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.ToTable("BillDetail");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__BillI__05D8E0BE");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__Produ__06CD04F7");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__Stock__04E4BC85");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Cashier>(entity =>
            {
                entity.ToTable("Cashier");

                entity.HasIndex(e => e.Username, "UQ__Cashier__536C85E43F04794A")
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
                    .HasConstraintName("FK__Cashier__StoreId__6B24EA82");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Category__BrandI__6E01572D");
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
                    .HasConstraintName("FK__DailyReve__Brand__10566F31");

                entity.HasOne(d => d.Store)
                    .WithMany()
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DailyReve__Store__0F624AF8");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EventName).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__BrandId__09A971A2");
            });

            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.ProductId })
                    .HasName("PK__EventDet__B204047C89EAA0E2");

                entity.ToTable("EventDetail");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDetails)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventDeta__Event__0C85DE4D");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.EventDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventDeta__Produ__0D7A0286");
            });

            modelBuilder.Entity<FcmtokenMobile>(entity =>
            {
                entity.ToTable("FCMTokenMobile");

                entity.Property(e => e.TokenId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FcmtokenMobiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FCMTokenM__UserI__123EB7A3");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__BrandId__71D1E811");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Product__Categor__70DDC3D8");

                entity.HasOne(d => d.UnpackedProduct)
                    .WithMany(p => p.InverseUnpackedProduct)
                    .HasForeignKey(d => d.UnpackedProductId)
                    .HasConstraintName("FK__Product__Unpacke__72C60C4A");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Receipt__StoreId__75A278F5");
            });

            modelBuilder.Entity<ReceiptDetail>(entity =>
            {
                entity.HasKey(e => new { e.ReceiptId, e.ProductId })
                    .HasName("PK__ReceiptD__0748084C45C23191");

                entity.ToTable("ReceiptDetail");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReceiptDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptDe__Produ__797309D9");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.ReceiptDetails)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptDe__Recei__787EE5A0");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__ProductId__7C4F7684");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__ReceiptId__7E37BEF6");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__StoreId__7D439ABD");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Store__BrandId__6754599E");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "UQ__User__536C85E4CB167563")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "UQ__User__5C7E359EBBF7F3E3")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534B78CEFC2")
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
                    .HasName("PK__UserBran__EA2583497B5AC241");

                entity.ToTable("UserBrand");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.UserBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBrand__Brand__6477ECF3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBrands)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBrand__UserI__6383C8BA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
