using Microsoft.EntityFrameworkCore;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.DAL
{
    public class ParfumeContext : DbContext
    {
        public ParfumeContext(DbContextOptions<ParfumeContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CrediteHistory> CrediteHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<UserWebPushCredentials> userWebPushCredentials { get; set; }
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<BonusCard> BonusCards { get; set; }
        public DbSet<BonusCardType> BonusCardTypes { get; set; }
        public DbSet<BonusHistory> BonusHistories { get; set; }
        public DbSet<BonusCardHistory> BonusCardHistories { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SellerByOrderHistory> SellerByOrderHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(b => b.CreateDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Order>()
               .Property(b => b.CreateOn)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Order>()
               .Property(b => b.CreateDate)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Card>()
               .Property(b => b.CreateDate)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<CrediteHistory>()
                  .Property(b => b.CreateDate)
                  .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<BonusCardHistory>()
                  .Property(b => b.CreateDate)
                  .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<SellerByOrderHistory>()
                 .Property(b => b.CreateDate)
                 .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Customer>()
                .Property(b => b.CreateDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Log>()
                .Property(b => b.CreateDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Bonus>()
                .Property(b => b.CreateDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<BonusHistory>()
                .Property(b => b.CreateDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<BonusCard>()
               .Property(b => b.CreateDate)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Customer>()
               .Property(b => b.IsActive)
               .HasDefaultValue(true);
            modelBuilder.Entity<Card>()
               .Property(b => b.Active)
               .HasDefaultValue(true);
            modelBuilder.Entity<BonusCard>()
               .Property(b => b.IsActive)
               .HasDefaultValue(true);
            modelBuilder.Entity<Seller>()
               .Property(b => b.IsActive)
               .HasDefaultValue(true);
            modelBuilder.Entity<Customer>()
               .Property(b => b.IsBlock)
               .HasDefaultValue(false);
            modelBuilder.Entity<Order>()
              .Property(x => x.Price)
              .HasPrecision(25, 4);
            //modelBuilder.Entity<Order>()
            // .Property(x => x.MonthPrice)
            // .HasPrecision(25, 4);
            modelBuilder.Entity<Order>()
                   .Property(x => x.TotalPrice)
                   .HasPrecision(25, 4);
            modelBuilder.Entity<CrediteHistory>()
                .Property(x => x.CachMany)
                .HasPrecision(25, 4);
        }
    }
}
