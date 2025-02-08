using System;
using _04LibraryWeb.Models;

namespace _04LibraryWeb.Services;

public interface IApiService
{
    Task<ApiResponse> PostAsync<T1, T2>(string endpointPath, T1 model);

    Task<ApiResponse> GetWithAuth(string endpointPath, string token);

    Task<ApiResponse> PutAsync<T1, T2>(string endpointPath, T1 model);

}
