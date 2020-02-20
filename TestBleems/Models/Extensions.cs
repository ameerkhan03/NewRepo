using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestBleems.Models
{
#pragma warning disable CS1591
	public static  class TeamBleemsDbContextExtensions
	{
		public static IQueryable<Product> GetProductList(this TeamBleemsDbContext dbContext, int pageSize = 10, int pageNumber = 1, string productName = null, int? categoryID = null, int? isAll = 0)
		{
			// Get query from DbSet
			var query = dbContext.Product.AsQueryable();

			// Filter by: 'category'
			if (categoryID.HasValue)
				query = query.Where(item => item.categoryId == categoryID);

			// Filter by: 'productName'
			if (!string.IsNullOrEmpty(productName))
				query = query.Where(item => item.productName.Contains(productName));

			// Filter by: 'isActive'
			if (isAll.HasValue)
			{
				if (isAll == -1)
					query = query.Where(item => item.isActive == false);
				else if (isAll == 1)
					query = query.Where(item => item.isActive == true);
			}
			return query;
		}

		public static async Task<Product> GetProductAsync(this TeamBleemsDbContext dbContext, Product entity)
		   => await dbContext.Product.FirstOrDefaultAsync(item => item.productId == entity.productId);

		public static async Task<Product> GetProductByProductNameAsync(this TeamBleemsDbContext dbContext, Product entity)
			=> await dbContext.Product.FirstOrDefaultAsync(item => item.productName == entity.productName);

		public static async Task<Category> GetCategoryAsync(this TeamBleemsDbContext dbContext, Category entity)
		   => await dbContext.Category.FirstOrDefaultAsync(item => item.categoryId == entity.categoryId);

	}

	public static class IQueryableExtensions
	{
		public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 0, int pageNumber = 0) where TModel : class
			=> pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
	}
#pragma warning restore CS1591
}
