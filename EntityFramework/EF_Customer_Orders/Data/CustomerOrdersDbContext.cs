using EF_Customer_Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_Customer_Orders.Data
{
    public class CustomerOrdersDbContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public CustomerOrdersDbContext(DbContextOptions<CustomerOrdersDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique and required business keys
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.Property(e => e.AgentCode)
                    .IsRequired();

                entity.HasIndex(e => e.AgentCode)
                    .IsUnique();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustCode)
                    .IsRequired();

                entity.HasIndex(e => e.CustCode)
                    .IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrdNum)
                    .IsRequired();

                entity.HasIndex(e => e.OrdNum)
                    .IsUnique();
            });

            // Agent - Customer
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Agent)
                .WithMany(a => a.Customers)
                .HasForeignKey(c => c.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Agent - Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Agent)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AgentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}