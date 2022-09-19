namespace ASK.Shared.Models;

public class StockModel : ModelBase
{
	//stock
	public int Quantity { get; set; }

	public DateTime? ExpDate { get; set; }

	public DateTime? ReceiveDate { get; set; }

	public string? LotNumber { get; set; }

	public bool? Quarantaine { get; set; }


	//product
	public ProductModel Product { get; set; } = new ProductModel();


	//location
	public LocationModel Location { get; set; } = new LocationModel();

	//used for moving stock
	public bool Selected { get; set; } = false;

	public int QuantityToMove { get; set; }
}