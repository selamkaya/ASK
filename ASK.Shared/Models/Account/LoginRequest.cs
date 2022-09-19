using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models.Account;

public class LoginRequest
{
	public string Type { get; set; } = "Email";

	[Required]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string Password { get; set; } = string.Empty;
}
