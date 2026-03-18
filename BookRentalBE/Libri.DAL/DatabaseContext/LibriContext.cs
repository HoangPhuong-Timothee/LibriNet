using Libri.DAL.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Libri.DAL.DatabaseContext
{
    public partial class LibriContext : DbContext
    {
        public LibriContext()
        {

        }

        public LibriContext(DbContextOptions<LibriContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookImage> BookImages { get; set; } = null!;
        public virtual DbSet<BookStore> BookStores { get; set; } = null!;
        public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; } = null!;
        public virtual DbSet<ExportHistory> ExportHistories { get; set; } = null!;
        public virtual DbSet<ExportInventoryReceipt> ExportInventoryReceipts { get; set; } = null!;
        public virtual DbSet<ExportReceiptItem> ExportReceiptItems { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<ImportHistory> ImportHistories { get; set; } = null!;
        public virtual DbSet<ImportInventoryReceipt> ImportInventoryReceipts { get; set; } = null!;
        public virtual DbSet<ImportReceiptItem> ImportReceiptItems { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<InventoryAudit> InventoryAudits { get; set; } = null!;
        public virtual DbSet<InventoryAuditDetail> InventoryAuditDetails { get; set; } = null!;
        public virtual DbSet<InventoryAuditResult> InventoryAuditResults { get; set; } = null!;
        public virtual DbSet<LibriLog> LibriLogs { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<UnitMapping> UnitMappings { get; set; } = null!;
        public virtual DbSet<UnitOfMeasure> UnitOfMeasures { get; set; } = null!;
        public virtual DbSet<UserAuth> UserAuths { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ__Addresse__1788CC4DB0B3205A")
                    .IsUnique();

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.District).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.Street).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Ward).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Address)
                    .HasForeignKey<Address>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addresses_UserInfo");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasIndex(e => e.Name, "IDX_AuthorName");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Updating/Unknown')");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Title, "IDX_BookTitle");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.Description).UseCollation("Vietnamese_CI_AS");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('https://www.scottishpoetrylibrary.org.uk/wp-content/uploads/2018/05/SnipImage_6-480x360.jpg')");

                entity.Property(e => e.IsAvailable).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(255)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .UseCollation("Vietnamese_CI_AS");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Authors");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Books_Genres");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Publishers");
            });

            modelBuilder.Entity<BookImage>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ImageIdSeq])");

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.PublicId).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookImages)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookImages_Books");
            });

            modelBuilder.Entity<BookStore>(entity =>
            {
                entity.HasIndex(e => e.StoreName, "IDX_StoreName");

                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [BookStoreId])");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<DeliveryMethod>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.DeliveryTime).HasMaxLength(100);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ShortName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<ExportHistory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ExportHistoryIdSeq])");

                entity.Property(e => e.ExportDate).HasColumnType("datetime");

                entity.Property(e => e.PerformedBy).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ExportHistories)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ExportHistories_Books");

                entity.HasOne(d => d.BookStore)
                    .WithMany(p => p.ExportHistories)
                    .HasForeignKey(d => d.BookStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ExportHistories_BookStores");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.ExportHistories)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ExportHistories_UnitOfMeasureId");
            });

            modelBuilder.Entity<ExportInventoryReceipt>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ExportInventoryReceiptIdSeq])");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.ExportNotes).HasMaxLength(255);

                entity.Property(e => e.ExportReceiptCode).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<ExportReceiptItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ExportInventoryReceiptItemIdSeq])");

                entity.HasOne(d => d.ExportReceipt)
                    .WithMany(p => p.ExportReceiptItems)
                    .HasForeignKey(d => d.ExportReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExportReceiptItems_ExportInventoryReceipts");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasIndex(e => e.Name, "IDX_GenreName");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<ImportHistory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ImportHistoryIdSeq])");

                entity.Property(e => e.ImportDate).HasColumnType("datetime");

                entity.Property(e => e.PerformedBy).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ImportHistories)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ImportHistories_Books");

                entity.HasOne(d => d.BookStore)
                    .WithMany(p => p.ImportHistories)
                    .HasForeignKey(d => d.BookStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ImportHistories_BookStores");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.ImportHistories)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ImportHistories_UnitOfMeasureId");
            });

            modelBuilder.Entity<ImportInventoryReceipt>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ImportInventoryReceiptIdSeq])");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.ImportNotes).HasMaxLength(255);

                entity.Property(e => e.ImportReceiptCode).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<ImportReceiptItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [ImportInventoryReceiptItemIdSeq])");

                entity.HasOne(d => d.ImportReceipt)
                    .WithMany(p => p.ImportReceiptItems)
                    .HasForeignKey(d => d.ImportReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportReceiptItems_ImportInventoryReceipts");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasIndex(e => new { e.BookId, e.BookStoreId }, "IX_Inventories_BookStoreId");

                entity.HasIndex(e => e.BookId, "idx_inventories_bookid");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventories_Books");

                entity.HasOne(d => d.BookStore)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.BookStoreId)
                    .HasConstraintName("FK_Inventories_BookStores");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .HasConstraintName("FK_Inventories_UnitOfMeasures");
            });

            modelBuilder.Entity<InventoryAudit>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [InventoryAuditIdSeq])");

                entity.Property(e => e.AuditCode).HasMaxLength(255);

                entity.Property(e => e.AuditDate).HasColumnType("datetime");

                entity.Property(e => e.AuditNotes).HasMaxLength(255);

                entity.Property(e => e.AuditStatus).HasMaxLength(50);

                entity.Property(e => e.AudittedBy).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<InventoryAuditDetail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [InventoryAuditDetailsIdSeq])");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(255)
                    .HasColumnName("ISBN");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.InventoryAuditDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditDetails_Books");

                entity.HasOne(d => d.BookStore)
                    .WithMany(p => p.InventoryAuditDetails)
                    .HasForeignKey(d => d.BookStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditDetails_BookStores");

                entity.HasOne(d => d.InventoryAudit)
                    .WithMany(p => p.InventoryAuditDetails)
                    .HasForeignKey(d => d.InventoryAuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditDetails_InventoryAudits");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.InventoryAuditDetails)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditDetails_UnitOfMeasures");
            });

            modelBuilder.Entity<InventoryAuditResult>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [InventoryAuditResultIdSeq])");

                entity.Property(e => e.ConductedAt).HasColumnType("datetime");

                entity.Property(e => e.ConductedBy).HasMaxLength(255);

                entity.Property(e => e.ResultDetails).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.InventoryAuditResults)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditResults_Books");

                entity.HasOne(d => d.BookStore)
                    .WithMany(p => p.InventoryAuditResults)
                    .HasForeignKey(d => d.BookStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditResults_BookStores");

                entity.HasOne(d => d.InventoryAuditDetails)
                    .WithMany(p => p.InventoryAuditResults)
                    .HasForeignKey(d => d.InventoryAuditDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditResults_InventoryAuditDetails");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.InventoryAuditResults)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryAuditResults_UnitOfMeasures");
            });

            modelBuilder.Entity<LibriLog>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [OrderSeq])");

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.District).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.Street).HasMaxLength(255);

                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UserEmail).HasMaxLength(255);

                entity.Property(e => e.Ward).HasMaxLength(255);

                entity.HasOne(d => d.DeliveryMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_DeliveryMethods");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [OrderItemSeq])");

                entity.Property(e => e.BookTitle).HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderItems_Orders");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasIndex(e => e.Name, "IDX_PublisherName");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('Updating/Unknown')");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title).HasMaxLength(20);
            });

            modelBuilder.Entity<UnitMapping>(entity =>
            {
                entity.HasKey(e => new { e.SrcUnitId, e.DestUnitId })
                    .HasName("PK__UnitMapp__1230A779431DD2A3");

                entity.Property(e => e.DestUnitRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.SrcUnitRate).HasColumnType("decimal(18, 6)");

                entity.HasOne(d => d.DestUnit)
                    .WithMany(p => p.UnitMappingDestUnits)
                    .HasForeignKey(d => d.DestUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitMappings_DestUnit");

                entity.HasOne(d => d.SrcUnit)
                    .WithMany(p => p.UnitMappingSrcUnits)
                    .HasForeignKey(d => d.SrcUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitMappings_SrcUnit");
            });

            modelBuilder.Entity<UnitOfMeasure>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(NEXT VALUE FOR [UnitOfMeasureSeq])");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.HasKey(e => e.UserInfoId)
                    .HasName("PK__UserAuth__D07EF2E4012D57DE");

                entity.ToTable("UserAuth");

                entity.HasIndex(e => e.Email, "IX_UserAuth_Email");

                entity.HasIndex(e => e.UserInfoId, "UC_UserInfoId")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__UserAuth__A9D10534C7EE0EF7")
                    .IsUnique();

                entity.Property(e => e.UserInfoId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.LastLoggedIn).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PasswordSalt).HasMaxLength(255);

                entity.HasOne(d => d.UserInfo)
                    .WithOne(p => p.UserAuth)
                    .HasForeignKey<UserAuth>(d => d.UserInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserInfoId");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UserInfo");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.ImagePublicId).HasMaxLength(255);

                entity.Property(e => e.ImageUrl).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(11);

                entity.Property(e => e.UserName).HasMaxLength(255);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserRole__RoleId__1E5A75C5"),
                        r => r.HasOne<UserInfo>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserRole__UserId__1D66518C"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760AD9E7A240B");

                            j.ToTable("UserRole");
                        });
            });

            modelBuilder.HasSequence<int>("BookStoreId");

            modelBuilder.HasSequence<int>("ExportHistoryIdSeq");

            modelBuilder.HasSequence("ExportInventoryReceiptIdSeq");

            modelBuilder.HasSequence("ExportInventoryReceiptItemIdSeq");

            modelBuilder.HasSequence("ImageIdSeq");

            modelBuilder.HasSequence<int>("ImportHistoryIdSeq");

            modelBuilder.HasSequence("ImportInventoryReceiptIdSeq");

            modelBuilder.HasSequence("ImportInventoryReceiptItemIdSeq");

            modelBuilder.HasSequence<int>("InventoryAuditDetailsIdSeq");

            modelBuilder.HasSequence<int>("InventoryAuditIdSeq");

            modelBuilder.HasSequence<int>("InventoryAuditResultIdSeq");

            modelBuilder.HasSequence("OrderItemSeq");

            modelBuilder.HasSequence("OrderSeq");

            modelBuilder.HasSequence<int>("RefreshTokenIdSeq");

            modelBuilder.HasSequence("UnitOfMeasureSeq").StartsAt(2);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
