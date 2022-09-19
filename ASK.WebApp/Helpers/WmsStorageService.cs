using ASK.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ASK.WebApp.Helpers;

public class WmsStorageService : IWmsStorageService
{
	private ProtectedLocalStorage _protectedLocalStore;

	public WmsStorageService(ProtectedLocalStorage protectedLocalStore)
	{
		_protectedLocalStore = protectedLocalStore;
	}

	public async Task<string> GetItem(string key)
	{
		try
		{
			//var item = await _protectedLocalStore
			//	.GetAsync<string>( key );

			var item = await _protectedLocalStore
				.GetAsync<string>(key)
				.AsTask()
				.ConfigureAwait(false);

			return item.Value ?? string.Empty;
		}
		catch (Exception)
		{
			//throw new Exception( "Cant find item" );
			return string.Empty;
		}
		//return "";
	}

	public async Task SetItem(string key, string value)
	{
		try
		{
			await _protectedLocalStore.SetAsync(key, value);
		}
		catch (Exception)
		{
			throw new Exception("Cant find item");
		}
	}

	public async Task RemoveItem(string key)
	{
		try
		{
			await _protectedLocalStore.DeleteAsync(key);
		}
		catch (Exception)
		{
			//throw new Exception( "Cant find item" );
		}
	}

	public async Task<string> Test2()
	{
		return await Task.FromResult("");
	}
}
