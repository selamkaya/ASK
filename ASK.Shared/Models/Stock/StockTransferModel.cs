using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models.Stock;

public class StockTransferModel
{
	[Required]
	public int ProductId { get; set; }

	[Required]
	public int LocationFromId { get; set; }

	[Required]
	public int LocationToId { get; set; }

	[Required]
	public int Quantity { get; set; }
}
