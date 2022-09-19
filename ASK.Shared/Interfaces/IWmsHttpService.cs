using ASK.Shared.Models;

namespace ASK.Shared.Interfaces;

public interface IWmsHttpService
{
	Task<TResponse> Get<TResponse>(string uri);
	Task<TResponse> Post<TRequest, TResponse>(string uri, TRequest value) where TRequest : class;
	Task<AppResult<TResponse>> Login<TRequest, TResponse>(string uri, TRequest value) where TRequest : class;
}