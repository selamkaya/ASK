namespace ASK.Shared.Models.Job;

public class PickListModel
{
	public int ShipmentId { get; set; }

	public int ShipmentItemId { get; set; }

	public int LocationId { get; set; }

	public string LocationCode { get; set; }

	public string BinCode { get; set; }

	public int ProductId { get; set; }

	public string ProductSku { get; set; }

	public string ProductBarCode { get; set; }

	public string ProductName { get; set; }

	public string ProductVariantBarcodes { get; set; }

	public int? ImageId { get; set; }

	public decimal? Weight { get; set; }

	public string PickInstructions { get; set; }

	public int Quantity { get; set; }

	public int TrackingNumber { get; set; }

	public bool? Recount { get; set; }

}
