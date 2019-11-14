using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop2.Core.Models;

namespace MyShop2.DataAccess.SQL
{
	public class DataContext : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer<DataContext>(null);
			base.OnModelCreating(modelBuilder);
		}
		public DataContext() 
			: base("DefaultConnection") {


		}
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItems> BasketItems { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItems> OrderItems { get; set; }
	}

}
