using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Datamodel = TestBleems.Models;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace BleemsWeb.Controllers
{
	public class UserController : Controller
	{
		static readonly HttpClient client = new HttpClient();

		static readonly string apibaseUrl = "https://localhost:44309/api/v1/";
		public string getsessionName;
		 
		public IActionResult Index()
		{
			ViewBag.getsessionName = HttpContext.Session.GetString("SessionKeyName");
			return View();
		}

		public IActionResult Login()
		{
			if (HttpContext.Session.GetString("SessionKeyName") != null)
			{
				return RedirectToAction("Index", "Product");
			}
			else
				return View();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="inputdata"></param>
		/// <returns></returns>
		public string Response(string url, string inputdata)
		{
			try
			{
				WebClient client = new WebClient();
				client.Headers["Content-type"] = "application/json";
				client.Encoding = Encoding.UTF8;
				string json = client.UploadString(url, inputdata);

				return json;
			}
			catch (Exception ex)
			{
				return "";
			}


		}

		[HttpPost]
		public IActionResult Login(Datamodel.UserAdmin objAdmin)
		{
			if (objAdmin.userName == null || objAdmin.password == null)
			{
				if (objAdmin.userName == null)
				{
					ModelState.AddModelError("userName", "Enter your Username");
				}
				if (objAdmin.password == null)
				{
					ModelState.AddModelError("password", "Enter your Password");
				}
				return View();
			}
			string userInfo = JsonSerializer.Serialize(objAdmin);
			string apiresult = Response(apibaseUrl + "UserLogin/Login", userInfo);
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
					Datamodel.UserAdmin objAdmindata = new Datamodel.UserAdmin { userName = userinfo.userName, isActive = userinfo.isActive, userId = userinfo.userId };
					if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionKeyName")))
					{
						HttpContext.Session.SetString("SessionKeyName", objAdmindata.userName);

						ViewBag.getsessionName = HttpContext.Session.GetString("SessionKeyName");
					 
					}
					 
					return RedirectToAction("Index", "Product");
				}
				else
				{
					return View();
				}
			}
			else
			{
				return View();
			}
		}
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("login", "User");
		}


	}

}