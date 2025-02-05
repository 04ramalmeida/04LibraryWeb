using System;
using System.Net;
using System.Threading.Tasks;
using _04LibraryWeb.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace _04LibraryWeb.Services;

public class ApiService : IApiService
{
	
	private readonly HttpClient _httpClient;

	public ApiService(IHttpClientFactory factory)
	{
		_httpClient = factory.CreateClient("api");
	}

	public async Task<ApiResponse> LoginAsync(LoginViewModel model) 
	{
		_httpClient.BaseAddress = new Uri("https://localhost:7172/api/Auth/Login/");

		HttpResponseMessage response;
		
		try
		{
			response = await _httpClient.PostAsJsonAsync<LoginViewModel>(_httpClient.BaseAddress,
			 model, new JsonSerializerOptions(), default);
		}
		catch (HttpRequestException ex)
		{
			return new ApiResponse
			{
				IsSuccess = false,
				StatusCode = HttpStatusCode.ServiceUnavailable, 
				Message = ex.Message
			};
		}


		if (!response.IsSuccessStatusCode)
		{
			return new ApiResponse
			{
				IsSuccess = false,
				StatusCode = response.StatusCode,
				Message = response.ReasonPhrase
			};
		}
		
		var result = await response.Content.ReadAsStringAsync();
		
		var token = JsonConvert.DeserializeObject<Token>(result);
		
		
		ApiResponse apiResponse = new ApiResponse();
		
		apiResponse.IsSuccess = response.IsSuccessStatusCode;
		apiResponse.StatusCode = response.StatusCode;
		apiResponse.Message = response.ReasonPhrase;
		apiResponse.ApiObject = token;
		
		return apiResponse;
	}


}
