using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Datamodel = TestBleems.Models;

namespace BleemsWeb.Controllers
{

	public class ProductController : Controller
	{
		static readonly string apibaseUrl = "https://localhost:44309/api/v1/";
		const string SessionName = null;
		public string Response(string url, string inputdata)
		{
			try
			{
				WebClient client = new WebClient();
				client.Headers["Content-type"] = "application/json";
				client.Encoding = Encoding.UTF8;
				// string json = client.OpenRead(url);

				// WebClient webClient = new WebClient();
				Stream stream = client.OpenRead(url);

				// convert stream to string
				StreamReader reader = new StreamReader(stream);
				string json = reader.ReadToEnd();

				return json;
			}
			catch (Exception ex)
			{
				return "";
			}


		}
		public IActionResult Index()
		{
			if (HttpContext.Session.GetString("SessionKeyName") == null)
			{
				return RedirectToAction("Login", "User");
			}
			else
			{
				ViewBag.getsessionName = HttpContext.Session.GetString("SessionKeyName");
				string apiresult = Response(apibaseUrl + "Product/Product?pageSize=10&pageNumber=1", "");
				if (apiresult.Length > 0)
				{
					dynamic data = JObject.Parse(apiresult.ToString());
					string message = (data.message);
					bool isError = (bool)(data.didError);
					string errorMessage = (data.errorMessage);
					dynamic userinfo = data.model;
					ViewBag.errorMessage = errorMessage;
					if (!isError)
					{
						ViewBag.productList = data.model;
					}
					else
					{
						//   return View();
					}
				}
				return View();
			}
		}
		public ActionResult ModalPopUp()
		{
			//var model = new Contact { };

			return PartialView("ModalPopUp");
		}
	}
}