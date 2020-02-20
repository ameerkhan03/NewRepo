using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestBleems.Models;

namespace TestBleems.Controllers
{
#pragma warning disable CS1591
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ProductController : ControllerBase
	{
		protected readonly ILogger Logger;
		protected readonly TeamBleemsDbContext DbContext;

		public ProductController(ILoggerFactory DepLoggerFactory, TeamBleemsDbContext dbContext)
		{
			Logger = DepLoggerFactory.CreateLogger("Controllers.ProductController");
			DbContext = dbContext;
		}
#pragma warning restore CS1591

		/// <summary>
		///  Return product List 
		/// </summary>
		/// <param name="pageSize"></param>
		/// <param name="pageNumber"></param>
		/// <param name="productName"></param>
		/// <param name="categoryID"></param>
		/// <param name="isAll"></param>
		/// <returns></returns>
		[HttpGet("Product")]
		[ProducesResponseType(200)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetProductAsync(int pageSize = 10, int pageNumber = 1, string productName = null, int? categoryID = null, int? isAll = null)
		{
			Logger?.LogDebug("'{0}' has been invoked", nameof(GetProductAsync));

			var response = new PagedResponse<Product>();

			try
			{
				// Get the "proposed" query from repository
				var query = DbContext.GetProductList(pageSize,pageNumber,productName,categoryID,isAll);

				// Set paging values
				response.PageSize = pageSize;
				response.PageNumber = pageNumber;

				// Get the total rows
				response.ItemsCount = await query.CountAsync();

				// Get the specific page from database
				response.Model = await query.Paging(pageSize, pageNumber).ToListAsync();

				response.Message = string.Format("Page {0} of {1}, Total of products: {2}.", pageNumber, response.PageCount, response.ItemsCount);

				Logger?.LogInformation("The product list have been retrieved successfully.");
			}
			catch (Exception ex)
			{
				response.DidError = true;
				response.ErrorMessage = "There was an internal error, please contact to technical support.";
				Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetProductAsync), ex);
			}
			return response.ToHttpResponse();
		}

		/// <summary>
		/// Retrieves a Product  by productId
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		[HttpGet("Product/{productId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetProductAsync1(int productId)
		{
			Logger?.LogDebug("'{0}' has been invoked", nameof(GetProductAsync1));
			var response = new SingleResponse<Product>();
			try
			{
				// Get the Product by id
				response.Model = await DbContext.GetProductAsync(new Product(productId));
			}
			catch (Exception ex)
			{
				response.DidError = true;
				response.ErrorMessage = "There was an internal error, please contact to technical support.";

				Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetProductAsync1), ex);
			}

			return response.ToHttpResponse();
		}

		/// <summary>
		/// To insert 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("Product")]
		[ProducesResponseType(200)]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostProductAsync([FromBody]PostProductRequest request)
		{
			Logger?.LogDebug("'{0}' has been invoked", nameof(PostProductAsync));

			var response = new SingleResponse<Product>();

			try
			{
					var existingEntity = await DbContext.GetProductByProductNameAsync(new Product { productName = request.productName });

				if (existingEntity != null)
					ModelState.AddModelError("ProductName", "Product item name already exists");

				if (!ModelState.IsValid)
					return BadRequest();

				// Create entity from request model
				var entity = request.ToEntity();

				// Add entity to repository
				DbContext.Add(entity);

				// Save entity in database
				await DbContext.SaveChangesAsync();

				// Set the entity to response model
				response.Model = entity;
			}
			catch (Exception ex)
			{
				response.DidError = true;
				response.ErrorMessage = "There was an internal error, please contact to technical support.";

				Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(PostProductAsync), ex);
			}

			return response.ToHttpResponse();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="productName"></param>
		/// <param name="price"></param>
		/// <param name="description"></param>
		/// <param name="isActive"></param>
		/// <param name="image"></param>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		[HttpGet("ProductAdd")]
		[ProducesResponseType(200)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetProductAsync(string productName = null, decimal price = 1, string description = null, bool isActive = false, string image = null, int categoryId = 1)
		{
			Logger?.LogDebug("'{0}' has been invoked", nameof(PostProductAsync));

			var response = new SingleResponse<Product>();

			try
			{

				PostProductRequest request = new PostProductRequest();
				request.price = price;
				request.isActive = true;
				request.description = description;
				request.productName = productName;
				request.categoryId = 1;


				var existingEntity = await DbContext.GetProductByProductNameAsync(new Product { productName = request.productName });

				if (existingEntity != null)
					ModelState.AddModelError("ProductName", "Product item name already exists");

				if (!ModelState.IsValid)
					return BadRequest();


				// Create entity from request model
				var entity = request.ToEntity();

				// Add entity to repository
				DbContext.Add(entity);

				// Save entity in database
				await DbContext.SaveChangesAsync();

				// Set the entity to response model
				response.Model = entity;
				response.Message = "inserted successfully ";

			}
			catch (Exception ex)
			{
				response.DidError = true;
				response.ErrorMessage = "There was an internal error, please contact to technical support.";

				Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(PostProductAsync), ex);
			}

			return response.ToHttpResponse();
		}



		/// <summary>
		/// Update
		/// </summary>
		/// <param name="id"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPut("Product/{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutProductAsync(int id, [FromBody]PutProductRequest request)
		{
			Logger?.LogDebug("'{0}' has been invoked", nameof(PutProductAsync));

			var response = new Response();

			try
			{
				// Get stock item by id
				var entity = await DbContext.GetProductAsync(new Product(id));

				// Validate if entity exists
				if (entity == null)
					return NotFound();

				// Set changes to entity
				entity.productName = request.productName;
				entity.categoryId = request.categoryId;
				entity.image = request.image;
				entity.price = request.price;

				// Update entity in repository
				DbContext.Update(entity);

				// Save entity in database
				await DbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				response.DidError = true;
				response.ErrorMessage = "There was an internal error, please contact to technical support.";

				Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(PutProductAsync), ex);
			}

			return response.ToHttpResponse();
		}


	}

}