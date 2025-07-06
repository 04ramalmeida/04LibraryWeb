using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
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
        ApiResponse response, aviResponse;

        try
        {
            response = await _apiService.GetWithAuth("/api/user/user-info", token);
            aviResponse = (await _apiService.GetWithAuth("api/user/user-pic", token));
        }
        catch (Exception e)
        {
            switch (e.Message)
            { //TODO: Replace with proper error pages
                
                default:
                    return View("~/Views/Shared/Error.cshtml");
                    
            }
        }
        
        
        
        string avatarUrl = JsonConvert.DeserializeObject<string>(aviResponse.ApiObject.ToString());
        ViewBag.AvatarUrl = avatarUrl;
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


    
    public async Task<IActionResult> UploadAvatar(AvatarViewModel model)
    {
        string token = Request.Cookies["accessToken"];

        var content = new MultipartFormDataContent();
        MemoryStream memoryStream = new MemoryStream();
        await model.AvatarFile.CopyToAsync(memoryStream);
        byte[] imageArray = memoryStream.ToArray();
        content.Add(new ByteArrayContent(imageArray), "file", model.AvatarFile.FileName);
        //var formDataPair = new KeyValuePair<string, string>("file", string.Empty);
        
        try
        {
            var response = (await _apiService.PutFormAsyncWithAuth("api/user/user-pic", content, token));
        }
        catch (Exception e)
        {
            switch (e.Message)
            { //TODO: Replace with proper error pages
                
                default:
                    return View("~/Views/Shared/Error.cshtml");
                    
            }
        }

        return RedirectToAction("Index");
    }
}