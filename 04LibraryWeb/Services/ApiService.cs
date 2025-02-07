using System;
using System.Net;
using System.Net.Http.Headers;
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
		Uri address = new Uri(_httpClient.BaseAddress, "/api/Auth/Login");
		
		HttpResponseMessage response;
		
		try
		{
			response = await _httpClient.PostAsJsonAsync(address,
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
	
	public async Task<ApiResponse> GetWithAuth(string endpointPath, string token)
	{
		var address = new Uri(_httpClient.BaseAddress, endpointPath);
		
		HttpResponseMessage response;

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		try
		{
			response = await _httpClient.GetAsync(address);
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
		
		ApiResponse apiResponse = new ApiResponse();
		
		apiResponse.IsSuccess = response.IsSuccessStatusCode;
		apiResponse.StatusCode = response.StatusCode;
		apiResponse.Message = response.ReasonPhrase;
		var responseString = await response.Content.ReadAsStringAsync();
		if (responseString != null)
		{
			apiResponse.ApiObject = responseString;
		}
		
		
		return apiResponse;
	}
}
