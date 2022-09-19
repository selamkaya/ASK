namespace ASK.Shared.Models;

public class ShipmentModel : ModelBase
{
	//shipment
	public int OrderId { get; set; }

	public int JobId { get; set; }

	public List<ProductModel> Products { get; set; } = new List<ProductModel>();

	//order
	public string CompanyName { get; set; } = string.Empty;

	public string OrderNumber { get; set; } = string.Empty;

	public DateTime? OrderDate { get; set; }
}

