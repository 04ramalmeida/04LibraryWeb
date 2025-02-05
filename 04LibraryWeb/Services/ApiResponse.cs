using System;
using System.Net;

namespace _04LibraryWeb.Services;

public class ApiResponse
{

	public bool IsSuccess { get; set; }
	
	public HttpStatusCode StatusCode { get; set; }
	
	public string? Message { get; set; }
	
	public object? ApiObject { get; set; }

}
