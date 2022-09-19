using ASK.Shared.Interfaces;
using ASK.Shared.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ASK.WebApp.Helpers;

public class WmsHttpService : IWmsHttpService
{
	private HttpClient _httpClient;
	private readonly IWmsStorageService _storageService;
	private IHttpContextAccessor _httpContextAccessor;

	public WmsHttpService(HttpClient httpClient, IWmsStorageService storageService, IHttpContextAccessor httpContextAccessor)
	{
		_httpClient = httpClient;
		_storageService = storageService;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<TResponse> Get<TResponse>(string uri)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, uri);
		return await SendRequest<TResponse>(request);
	}

	public async Task<TResponse> Post<TRequest, TResponse>(string uri, TRequest value) where TRequest : class
	{
		//value.IpAddress = "1.1.1.1";//_httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
		var request = new HttpRequestMessage(HttpMethod.Post, uri);
		request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
		return await SendRequest<TResponse>(request, false);
	}

	public async Task<AppResult<TResponse>> Login<TRequest, TResponse>(string uri, TRequest value) where TRequest : class
	{
		try
		{
			//value.IpAddress = "1.1.1.1";//_httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
			var request = new HttpRequestMessage(HttpMethod.Post, uri);
			request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
			return await SendRequest<AppResult<TResponse>>(request, true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}
	}

	private async Task<T> SendRequest<T>(HttpRequestMessage request, bool isLogin = false)
	{
		try
		{
			string token = !isLogin ? await _storageService.GetItem(Constants.TokenName) : "";
			if (!string.IsNullOrEmpty(token))
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
				throw new Exception(error["message"]);
			}

			return await response.Content.ReadFromJsonAsync<T>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}
	}
}

