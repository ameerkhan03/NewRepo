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
	public class UserLoginController : ControllerBase
	{
		protected readonly ILogger Logger;
		protected readonly TeamBleemsDbContext DbContext;

		public UserLoginController(ILoggerFactory DepLoggerFactory, TeamBleemsDbContext dbContext)
		{
			Logger = DepLoggerFactory.CreateLogger("Controllers.UserLoginController");
			DbContext = dbContext;
		}
#pragma warning restore CS1591

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("Login")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Login([FromBody]PostLoginRequest request)

		{

			Logger?.LogDebug("'{0}' has been invoked", nameof(Login));
			var response = new SingleResponse<UserAdmin>();

			try
			{
				var existingEntity = DbContext.UserAdmin.SingleAsync(x => x.userName == request.userName);
				if (existingEntity != null && existingEntity.Result.password == request.password && existingEntity.Result.isActive == true)
				{
					response.Model =
						new UserAdmin
						{
							userName = existingEntity.Result.userName,
							userId = existingEntity.Result.userId,
							isActive = existingEntity.Result.isActive
						};
				}
				else
				{
					response.DidError = true;
					if (existingEntity == null)
					{
						response.ErrorMessage = "User doesn't exists.";
					}
					else if (existingEntity.Result.password != request.password)
					{
						response.ErrorMessage = "you have entered wrong password.";
					}
					else
					{
						response.ErrorMessage = "you have Account is not activate yet.";
					}
				}
			}
			catch (Exception ex)
			{

				response.DidError = true;
				response.ErrorMessage = "There was an internal error, please contact to technical support.";

				Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Login), ex);
			}

			return response.ToHttpResponse();


		}
	}
}