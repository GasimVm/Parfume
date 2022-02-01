using Microsoft.EntityFrameworkCore;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.DAL
{
    public class ParfumeContext:DbContext
    {
        public ParfumeContext(DbContextOptions<ParfumeContext> options):base(options)
        {

        }
        public DbSet<User>  Users { get; set; }
        public DbSet<Order>  Orders { get; set; }
        public DbSet<CrediteHistory>  CrediteHistories { get; set; }
        public DbSet<Customer>  Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<PaymentHistory>  PaymentHistories { get; set; }
        public DbSet<UserWebPushCredentials>  userWebPushCredentials { get; set; }

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
               .Property(b => b.CreateDate)
               .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CrediteHistory>()
          .Property(b => b.CreateDate)
          .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Customer>()
        .Property(b => b.CreateDate)
        .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Log>()
       .Property(b => b.CreateDate)
       .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Customer>()
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
