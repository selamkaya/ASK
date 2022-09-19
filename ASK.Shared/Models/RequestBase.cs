using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models;

public abstract class RequestBase
{
	[Required]
	public int UserId { get; set; } = 0;

	[Required]
	public string IpAddress { get; set; } = string.Empty;

	[Required]
	public string Language { get; set; } = string.Empty;
}
