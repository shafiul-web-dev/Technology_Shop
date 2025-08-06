using Microsoft.EntityFrameworkCore;
using Technology_Shop.Models;

namespace Technology_Shop.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		// Tables
		public DbSet<User> Users { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Payment> Payments { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// User
			builder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();

			builder.Entity<User>()
				.HasMany(u => u.Orders)
				.WithOne(o => o.User)
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Admin
			builder.Entity<Admin>()
				.HasIndex(a => a.Email)
				.IsUnique();

			// Category
			builder.Entity<Category>()
				.HasIndex(c => c.Name)
				.IsUnique();

			builder.Entity<Category>()
				.HasMany(c => c.Products)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryID)
				.OnDelete(DeleteBehavior.Restrict);

			// Product
			builder.Entity<Product>()
				.HasMany(p => p.OrderItems)
				.WithOne(oi => oi.Product)
				.HasForeignKey(oi => oi.ProductID)
				.OnDelete(DeleteBehavior.Cascade);

			//  Order
			builder.Entity<Order>()
				.HasMany(o => o.OrderItems)
				.WithOne(oi => oi.Order)
				.HasForeignKey(oi => oi.OrderID)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Order>()
				.HasOne(o => o.Payment)
				.WithOne(p => p.Order)
				.HasForeignKey<Payment>(p => p.OrderID)
				.OnDelete(DeleteBehavior.Cascade);

			// OrderItem
			builder.Entity<OrderItem>()
				.HasKey(oi => oi.ID);

			// Payment
			builder.Entity<Payment>()
				.Property(p => p.PaymentMethod)
				.IsRequired()
				.HasMaxLength(50);

			base.OnModelCreating(builder);
		}
	}

}
