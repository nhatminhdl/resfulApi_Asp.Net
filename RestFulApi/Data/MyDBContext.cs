using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) {}

        #region DBSet
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<OrderBill> OrderBills { get; set; }
        public DbSet<DetailOrderBill> DetailOrderBills { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderBill>(entity => {
                entity.ToTable("OrderBill");
                entity.HasKey(OB => OB.OrderCode);
                entity.Property(OB => OB.DateOrder).HasDefaultValueSql("getutcdate()");
                entity.Property(OB => OB.Receiver).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<DetailOrderBill>(entity =>
            {
                entity.ToTable("DetailOrderBill");
                entity.HasKey(DOB => new { DOB.OrderCode, DOB.ProductCode });

                entity.HasOne(DOB => DOB.OrderBill)
                    .WithMany(DOB => DOB.DetailOrderBills)
                    .HasForeignKey(DOB => DOB.OrderCode)
                    .HasConstraintName("FK_DetailOrderBill_Order");

                entity.HasOne(DOB => DOB.Products)
                   .WithMany(DOB => DOB.DetailOrderBills)
                   .HasForeignKey(DOB => DOB.ProductCode)
                   .HasConstraintName("FK_DetailOrderBill_Product");
            });

            modelBuilder.Entity<Users>(entity => {

                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });
        }
    }
}
