namespace ASK.Shared.Models.Location;

public class LocationInfoResponse : ModelBase
{
	public string Code { get; set; } = string.Empty;

	public string Type { get; set; } = string.Empty;

	public bool IsActive { get; set; }

	public List<StockModel> Stocks { get; set; } = new List<StockModel>();
}