using System.Net;
using Microsoft.AspNetCore.Mvc;
using _04LibraryWeb.Models;
using _04LibraryWeb.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace _04LibraryWeb.Controllers
{
	public class AuthController : Controller
	{
		
		private readonly IApiService _apiService;


		public AuthController(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ActionResult> Login()
		{
			return await CheckIfAuth();
		}
		
		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel model) 
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Invalid login attempt");
				return View(model);
			}
			
			ApiResponse result = await _apiService.PostAsync<LoginViewModel, Token>("api/auth/login",model);

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
			
			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Register()
		{
			return await CheckIfAuth();
		}

		[HttpPost]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Registration failed.");
				return View(model);
			}

			ApiResponse result = await _apiService.PostAsync<RegisterViewModel, string>("api/auth/register",model);
			
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
				return View(model);
			}
			
			ViewBag.Message = "Registration successful. Check your email to confirm.";
			return View(model);
		}

		public async Task<ActionResult> ConfirmEmail()
		{
			return await CheckIfAuth();
		}

		[HttpPost]
		public async Task<ActionResult> ConfirmEmail(ConfirmViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Confirmation failed..");
				return View(model);
			}

			ApiResponse result = await _apiService.PutAsync<ConfirmViewModel, string>("api/auth/confirm",model);
			
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
				return View(model);
			}
			
			ViewBag.Message = "Confirmation successful. You may now log in.";
			return View(model);
		}

		public async Task<ActionResult> ForgotPassword()
		{	
			return await CheckIfAuth();
		}

		[HttpPost]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Invalid information.");
				return View(model);
			}

			ApiResponse result = await _apiService.PostAsync<string, string>("api/auth/forgot-password", model.Email);
			
			if (!result.IsSuccess)
			{
				switch (result.StatusCode)
				{
					case HttpStatusCode.ServiceUnavailable:
						ModelState.AddModelError("", "Service unavailable.");
						break;
					case HttpStatusCode.BadRequest:
						ModelState.AddModelError("", "Wrong email.");
						break;
					default:
						ModelState.AddModelError("", "Something went wrong.");
						break;
				}
				return View(model);
			}
			
			ViewBag.Message = "Check your email and follow the instructions.";
			return View(model);
		}
		
		public async Task<ActionResult> ResetPassword()
		{	
			return await CheckIfAuth();
		}

		[HttpPost]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Invalid information.");
				return View(model);
			}

			ApiResponse result = await _apiService.PutAsync<ResetPasswordViewModel, string>("api/auth/reset-password", model);
			
			if (!result.IsSuccess)
			{
				switch (result.StatusCode)
				{
					case HttpStatusCode.ServiceUnavailable:
						ModelState.AddModelError("", "Service unavailable.");
						break;
					case HttpStatusCode.BadRequest:
						ModelState.AddModelError("", "Wrong email or token.");
						break;
					default:
						ModelState.AddModelError("", "Something went wrong.");
						break;
				}
				return View(model);
			}
			
			ViewBag.Message = "Your password was successfully reset.";
			return View(model);
		}
		
		private async Task<ActionResult> CheckIfAuth()
		{
			string token = Request.Cookies["accessToken"];
			
			bool hasVerified = (await _apiService.GetWithAuth("/api/auth/verify-login",token)).IsSuccess;

			if (hasVerified)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}
		
		
	}
}


