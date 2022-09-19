namespace ASK.Shared.Models.Account;

public class LoginResponse
{
	public int UserId { get; set; }

	public string UserName { get; set; } = string.Empty;

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public string Token { get; set; } = string.Empty;

}
