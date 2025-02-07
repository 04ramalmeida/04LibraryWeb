using _04LibraryWeb.Models;
using _04LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _04LibraryWeb.ViewComponents;

public class UserViewComponent : ViewComponent
{
    private readonly IApiService _apiService;

    public UserViewComponent(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string token = Request.Cookies["accessToken"];
        if (token == null)
        {
            return View();
        }

        var userInfoString = (await _apiService.GetWithAuth("api/user/user-info", token)).ApiObject.ToString();
        UserInfoViewModel userInfo = JsonConvert.DeserializeObject<UserInfoViewModel>(userInfoString);
        ViewBag.UserName = userInfo.FirstName + " " + userInfo.LastName;
        return View();
    }
}