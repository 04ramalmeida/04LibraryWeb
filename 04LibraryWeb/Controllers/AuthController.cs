using System.Net;
using Microsoft.AspNetCore.Mvc;
using _04LibraryWeb.Models;
using _04LibraryWeb.Services;
using System.Threading.Tasks;

namespace _04LibraryWeb.Controllers
{
	public class AuthController : Controller
	{
		
		private readonly IApiService _apiService;


		public AuthController(IApiService apiService)
		{
			_apiService = apiService;
		}

		public ActionResult Login()
		{
			return View();

		}
		
		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel model) 
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Invalid login attempt");
				return View(model);
			}
			
			ApiResponse result = await _apiService.LoginAsync(model);

			if (!result.IsSuccess)
			{
				switch (result.StatusCode)
				{
					case HttpStatusCode.ServiceUnavailable:
						ModelState.AddModelError("", "Service unavailable.");
						break;
					case HttpStatusCode.BadRequest:
						ModelState.AddModelError("", "Wrong username or password.");
						break;
					default:
						ModelState.AddModelError("", "Something went wrong.");
						break;
				}
				return View();
			}

			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				IsEssential = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Domain = "localhost",
				Expires = DateTimeOffset.Now.AddDays(10)
			};
			
			Token userToken = result.ApiObject as Token;
			
			Response.Cookies.Append("accessToken", userToken.AccessToken, cookieOptions);
			Response.Cookies.Append("tokenType", userToken.TokenType, cookieOptions);
			Response.Cookies.Append("userId", userToken.UserId, cookieOptions);
			
			return View();
		}

	}
}


