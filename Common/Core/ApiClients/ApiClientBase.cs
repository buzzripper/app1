using Dyvenix.App1.Common.Core.Exceptions;
using Dyvenix.App1.Common.Core.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dyvenix.App1.Common.Core.ApiClients;

public abstract class ApiClientBase
{
	#region Fields

	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonSerializerOptionsGet = new JsonSerializerOptions
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true
	};
	private readonly JsonSerializerOptions _jsonSerializerOptionsPost = new JsonSerializerOptions
	{
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	#endregion

	#region Ctors / Init

	public ApiClientBase(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_jsonSerializerOptionsGet.Converters.Add(new JsonStringEnumConverter());
	}

	#endregion

	#region Methods

	protected async Task<TResult> GetAsync<TResult>(string uri)
	{
		var httpResponse = await _httpClient.GetAsync(uri);

		if (!httpResponse.IsSuccessStatusCode)
			throw new HttpException((int)httpResponse.StatusCode, $"{(int)httpResponse.StatusCode} - {httpResponse.ReasonPhrase}");
		if (httpResponse.StatusCode == HttpStatusCode.NoContent)
			return default;
		var responseString = await httpResponse.Content.ReadAsStringAsync();

		var apiResponse = JsonSerializer.Deserialize<ApiResponse<TResult>>(responseString, _jsonSerializerOptionsGet);

		if (apiResponse.StatusCode >= 100)
			throw new ApiException(apiResponse.StatusCode, apiResponse.Message, apiResponse.CorrelationId);

		return apiResponse.Data;
	}

	#region Post

	protected async Task PostAsync(string uri, object payload)
	{
		await CallAsync(MethodType.Post, uri, payload);
	}

	protected async Task<TResult> PostAsync<TResult>(string uri, object payload)
	{
		return await CallAsync<TResult>(MethodType.Post, uri, payload);
	}

	protected async Task<string> PostAsyncStr(string uri, object payload)
	{
		return await CallAsyncStr(MethodType.Post, uri, payload);
	}

	#endregion

	#region Put

	protected async Task PutAsync(string uri, object payload)
	{
		await CallAsync(MethodType.Put, uri, payload);
	}

	protected async Task<TResult> PutAsync<TResult>(string uri, object payload)
	{
		return await CallAsync<TResult>(MethodType.Put, uri, payload);
	}

	protected async Task<string> PutAsyncStr(string uri, object payload)
	{
		return await CallAsyncStr(MethodType.Put, uri, payload);
	}

	#endregion

	#region Patch

	protected async Task PatchAsync(string uri, object payload)
	{
		await CallAsync(MethodType.Patch, uri, payload);
	}

	protected async Task<TResult> PatchAsync<TResult>(string uri, object payload)
	{
		return await CallAsync<TResult>(MethodType.Patch, uri, payload);
	}

	protected async Task<string> PatchAsyncStr(string uri, object payload)
	{
		return await CallAsyncStr(MethodType.Patch, uri, payload);
	}

	#endregion

	#region Call

	protected async Task CallAsync(MethodType methodType, string uri, object payload)
	{
		var responseString = await CallAsyncStr(methodType, uri, payload);

		var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseString, _jsonSerializerOptionsPost);

		if (apiResponse.StatusCode >= 100)
			throw new ApiException(apiResponse.StatusCode, apiResponse.Message, apiResponse.CorrelationId);
	}

	protected async Task<TResult> CallAsync<TResult>(MethodType methodType, string uri, object payload)
	{
		var responseString = await CallAsyncStr(methodType, uri, payload);

		var apiResponse = JsonSerializer.Deserialize<ApiResponse<TResult>>(responseString, _jsonSerializerOptionsPost);

		if (apiResponse.StatusCode >= 100)
			throw new ApiException(apiResponse.StatusCode, apiResponse.Message, apiResponse.CorrelationId);

		return apiResponse.Data;
	}

	protected async Task<string> CallAsyncStr(MethodType methodType, string uri, object payload)
	{
		var json = JsonSerializer.Serialize(payload, _jsonSerializerOptionsPost);
		using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

		HttpResponseMessage httpResponse = null;

		switch (methodType)
		{
			case MethodType.Post:
				httpResponse = await _httpClient.PostAsync(uri, stringContent);
				break;
			case MethodType.Put:
				httpResponse = await _httpClient.PutAsync(uri, stringContent);
				break;
			default:
				httpResponse = await _httpClient.PatchAsync(uri, stringContent);
				break;
		}

		if (!httpResponse.IsSuccessStatusCode)
			throw new HttpException((int)httpResponse.StatusCode, httpResponse.ReasonPhrase);

		return await httpResponse.Content.ReadAsStringAsync();
	}

	#endregion

	#endregion

	public enum MethodType
	{
		Post,
		Put,
		Patch
	}
}
