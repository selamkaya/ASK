namespace ASK.Shared.Models;

public class ProductModel : ModelBase
{
	public string Sku { get; set; } = string.Empty;

	public string? BarCode { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;
}