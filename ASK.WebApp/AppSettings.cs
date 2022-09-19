namespace ASK.WebApp;

public class AppSettings
{
	// Development | Test | Acceptation | Production
	public string Environment { get; set; } = "Development";
	public string IdentityApiUrl { get; set; } = string.Empty;
	public string ApiUrl { get; set; } = string.Empty;
}
