namespace ASK.Shared.Interfaces;

public interface IWmsStorageService
{
	Task<string> GetItem( string key );
	Task SetItem( string key, string value );
	Task RemoveItem( string key );
}