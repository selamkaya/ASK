using ASK.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace ASK.WebApp.Helpers;

public class WmsAuthStateProvider : AuthenticationStateProvider
{
	private IWmsStorageService _storageService;

	public WmsAuthStateProvider(IWmsStorageService storageService)
	{
		_storageService = storageService;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var identity = new ClaimsIdentity();

		try
		{
			var token = await _storageService.GetItem(Constants.TokenName);
			if (!string.IsNullOrEmpty(token))
			{
				//identity = new ClaimsIdentity( ParceClaimsFromJwt( token ), "jwt" );
				identity = new ClaimsIdentity(ParceClaimsFromJwt(token), "jwt", "name", "role");
				identity.AddClaims(ExtractClaims(token));
			}
		}
		catch (Exception ex)
		{
			//var identity = new ClaimsIdentity();
		}

		var user = new ClaimsPrincipal(identity);
		var state = new AuthenticationState(user);
		NotifyAuthenticationStateChanged(Task.FromResult(state));

		return state;
	}

	public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
	{
		var state = await GetAuthenticationStateAsync();
		var authenticationStateProviderUser = state.User;
		return authenticationStateProviderUser;
	}

	private IEnumerable<Claim> ExtractClaims(string jwtToken)
	{
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
		IEnumerable<Claim> claims = securityToken.Claims;
		return claims;
	}

	private static IEnumerable<Claim> ParceClaimsFromJwt(string jwt)
	{
		var payload = jwt.Split(new char[] { '.' })[0];
		var jsonBytes = ParseBase64(payload);
		var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
		return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
	}

	private static byte[] ParseBase64(string base64)
	{
		switch (base64.Length % 4)
		{
			case 2: base64 += "=="; break;
			case 3: base64 += "="; break;
		}

		return Convert.FromBase64String(base64);
	}
}
