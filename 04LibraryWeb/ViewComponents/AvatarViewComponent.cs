using _04LibraryWeb.Models;
using _04LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace _04LibraryWeb.ViewComponents;

public class AvatarViewComponent : ViewComponent
{
    private readonly IApiService  _apiService;
    
    public AvatarViewComponent(IApiService apiService)
    {
        _apiService = apiService;
    }

    /*public async Task<IViewComponentResult> InvokeAsync(IFormFile file)
    {
        try
        {
            if (file.Length > 0)
            {
                string token = Request.Cookies["accessToken"];
                var response = (await _apiService.PutAsyncWithAuth<IFormFile, string>("api/user/user-pic",file, token));
                ViewBag.AvatarUrl = response.ApiObject;
                return View();
            }
            // TODO: Replace with proper error pages
            return View("~/Views/Shared/Error.cshtml");
        }
        catch (Exception e)
        {
            // TODO: Replace with proper error pages
            return View("~/Views/Shared/Error.cshtml");
            
        }
    }*/

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string token = Request.Cookies["accessToken"];
        ApiResponse response;
        try
        {
            response = (await _apiService.GetWithAuth("api/user/user-pic", token));
        }
        catch (Exception e)
        {
            switch (e.Message)
            { //TODO: Replace with proper error pages
                
                default:
                    return View("~/Views/Shared/Error.cshtml");
                    
            }
        }
        AvatarViewModel model  = new AvatarViewModel
        {
            Url = response.ApiObject.ToString()
        };
        return View(model);
    }
    
   
}