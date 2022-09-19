namespace ASK.Shared.Models.Stock;

public class StockTransferRequest : RequestBase
{
	public List<StockTransferModel> Transfers { get; set; } = new();
}