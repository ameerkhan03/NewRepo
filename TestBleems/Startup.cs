using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestBleems.Controllers;
using TestBleems.Models;
using Swashbuckle.AspNetCore.Swagger;


namespace TestBleems
{
	public class Startup
	{
		/// <param name="env"></param>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public static IConfiguration StaticConfig { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddControllers();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Add configuration for DbContext
			// Use connection string from appsettings.json file
			services.AddDbContext<TeamBleemsDbContext>(options =>
			{
				options.UseSqlServer(Configuration["AppSettings:ConnectionString"]);
			});

			// Set up dependency injection for controller's logger
			services.AddScoped<ILogger, Logger<FileContentResult>>();

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				//options.SwaggerDoc("v1", new Info { Title = "WideWorldImporters API", Version = "v1" });
				 
					c.SwaggerDoc("v1",null);
					c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
				 
				// Get xml comments path
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

				// Set xml path
				c.IncludeXmlComments(xmlPath);
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("./v1/swagger.json", "Test.Web.Api");
				//c.RoutePrefix = string.Empty;
			});

			//app.UseMvc();
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
