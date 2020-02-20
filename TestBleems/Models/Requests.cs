using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TestBleems.Models
{
#pragma warning disable CS1591
	public class PostProductRequest
	{
		public int? productId { get; set; }
		[Required]
		public int? categoryId { get; set; }

		[Required]
		[StringLength(50)]
		public string productName { get; set; }

		[Required]
		[StringLength(50)]
		public string image { get; set; }
		public string description { get; set; }

		[Required]
		public decimal price { get; set; }

		[Required]
		public bool? isActive { get; set; }
	}
	public class PostLoginRequest
	{
		[Required]
		public string userName { get; set; }

		[Required]
		public string password { get; set; }
		 
	}
	public class PutProductRequest
	{
		[Required]
		[StringLength(50)]
		public string productName { get; set; }
		public string description { get; set; }
		public string image { get; set; }

		[Required]
		public int? categoryId { get; set; }

		[Required]
		public decimal? price { get; set; }
	}

	public static class Extensions
	{
		public static Product ToEntity(this PostProductRequest request)
			=> new Product
			{
				//productId = request.productId,
				productName = request.productName,
				categoryId = request.categoryId,
				isActive = request.isActive,
				description = request.description,
				image = request.image,
				price = request.price
			};
	}

#pragma warning restore CS1591
}
