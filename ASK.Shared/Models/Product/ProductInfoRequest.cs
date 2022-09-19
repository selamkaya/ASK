using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models.Product;

public class ProductInfoRequest : RequestBase
{
	[Required]
	public string BarCode { get; set; }
}