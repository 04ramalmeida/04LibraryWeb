using System;
using _04LibraryWeb.Models;

namespace _04LibraryWeb.Services;

public interface IApiService
{
    Task<ApiResponse> LoginAsync(LoginViewModel model);

    Task<ApiResponse> GetWithAuth(string endpointPath, string token);
}
