namespace ASK.Shared.Models.Product;

public class ProductInfoResponse
{
	public string Sku { get; set; } = string.Empty;

	public string BarCode { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	//public string Description { get; set; }

	public List<StockModel> Stocks { get; set; } = new List<StockModel>();
}