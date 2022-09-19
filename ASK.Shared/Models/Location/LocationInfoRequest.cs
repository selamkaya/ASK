using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models.Location;

public class LocationInfoRequest : RequestBase
{
	[Required]
	public string BarCode { get; set; } = string.Empty;
}