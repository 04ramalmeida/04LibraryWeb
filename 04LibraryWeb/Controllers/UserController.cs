using System.Net;
using _04LibraryWeb.Models;
using _04LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _04LibraryWeb.Controllers;

public class UserController : Controller
{
    
    private readonly IApiService _apiService;

    public UserController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        string token = Request.Cookies["accessToken"];
        ApiResponse response = await _apiService.GetWithAuth("/api/user/user-info", token);
        if (!response.IsSuccess)
        {
            switch (response.StatusCode)
            { //TODO: Replace with proper error pages
                case HttpStatusCode.ServiceUnavailable:
                    return View("~/Views/Shared/Error.cshtml");
                case HttpStatusCode.BadRequest:
                    return View("~/Views/Shared/Error.cshtml");
                case HttpStatusCode.Unauthorized:
                    return View("~/Views/Shared/Error.cshtml");
                default:
                    return View("~/Views/Shared/Error.cshtml");
                    
            }
        }
        UserInfoViewModel userInfo = JsonConvert.DeserializeObject<UserInfoViewModel>(response.ApiObject.ToString());
        return View(userInfo);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UserInfoViewModel model)
    {
        string token = Request.Cookies["accessToken"];
			
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid information.");
            return View(model);
        }

        ApiResponse result = await _apiService.PutAsyncWithAuth<UserInfoViewModel, string>
            ("api/user/user-info", model, token);
			
        if (!result.IsSuccess)
        {
            switch (result.StatusCode)
            { //TODO: Replace with proper error pages
                case HttpStatusCode.ServiceUnavailable:
                    return View("~/Views/Shared/Error.cshtml");
                case HttpStatusCode.BadRequest:
                    return View("~/Views/Shared/Error.cshtml");
                case HttpStatusCode.Unauthorized:
                    return View("~/Views/Shared/Error.cshtml");
                default:
                    return View("~/Views/Shared/Error.cshtml");
                    
            }
            
        }
			
        ViewBag.Message = "Your information has been successfully changed.";
        return View(model);
    }
    
    private async Task<ActionResult> CheckIfAuth()
    {
        string token = Request.Cookies["accessToken"];
			
        bool hasVerified = (await _apiService.GetWithAuth("/api/auth/verify-login",token)).IsSuccess;

        if (hasVerified)
        {
            return View();
        }
        return RedirectToAction("Index", "Home");
    }
}