using Blazored.LocalStorage;
using System.Security.Claims;
using System.Text.Json;

namespace SeeSharp.UI;

public class CustomAuthenStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthenStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        
        string token = await _localStorage.GetItemAsStringAsync("accessToken") ?? string.Empty;
        var identity = new ClaimsIdentity();
        if (!string.IsNullOrEmpty(token))
        {
            identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        }
        
        var user = new ClaimsPrincipal(identity);
        var authenState = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(authenState));

        return authenState;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}


