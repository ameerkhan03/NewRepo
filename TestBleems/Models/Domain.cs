using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestBleems.Models
{
#pragma warning disable CS1591
	
	/// <summary>
	/// Product Table
	/// </summary>
	public partial class Product
	{
		public Product()
		{
		}

		public Product(int? productID)
		{
			productId = productID;
		}
 
		public int? productId { get; set; }
		public int? categoryId { get; set; }
		public string image { get; set; }
		public string productName { get; set; }
		public string description { get; set; }
		public decimal? price { get; set; }
		public bool? isActive { get; set; } = true;

	}

	


	/// <summary>
	/// Category
	/// </summary>
	public partial class Category
	{
		public Category()
		{
		}
		public Category(int? categoryID)
		{
			categoryId = categoryID;
		}
		[Key]
		public int? categoryId { get; set; }
		public string categoryName { get; set; }
		public string userCreated { get; set; }
		public bool isActive { get; set; } = true;

	}


	/// <summary>
	/// UserAdmin Table
	/// </summary>
	public partial class UserAdmin
	{
		public UserAdmin()
		{
		}

		public UserAdmin(int? userID)
		{
			userId = userID;
		}
		[Key]
		public int? userId { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
		public bool isActive { get; set; }

	}

	
	/// <summary>
	/// Userprivileges
	/// </summary>
	public partial class Userprivileges
	{
		public Userprivileges()
		{
		}

		public Userprivileges(int? userPrivilegesID)
		{
			userPrivilegesId = userPrivilegesID;
		}
		[Key]
		public int? userPrivilegesId { get; set; }
		public int? userId { get; set; }
		public string pageName { get; set; }
		public bool isRead { get; set; }
		public bool isWrite { get; set; }

	}

	
	public class TeamBleemsDbContext : DbContext
	{
		public TeamBleemsDbContext(DbContextOptions<TeamBleemsDbContext> options)
			: base(options)
		{
		}

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	// Apply configurations for entity

		//	modelBuilder
		//		.ApplyConfiguration(new ProductConfiguration());

		//	base.OnModelCreating(modelBuilder);
		//}

		public DbSet<Product> Product { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<UserAdmin> UserAdmin { get; set; }
		public DbSet<Userprivileges> Userprivileges { get; set; }
	}
#pragma warning restore CS1591
}
