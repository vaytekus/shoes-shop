using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace User.Api.Infrastructure;

public class KeycloakAdminService(HttpClient http, IConfiguration config)
{
    private readonly string _realm = config["Keycloak:Realm"] ?? "shoes-shop";
    private readonly string _adminUser = config["Keycloak:AdminUsername"] ?? "admin";
    private readonly string _adminPasword = config["Keycloak:AdminPassword"] ?? "shoes-shop";

    private async Task<string?> GetAdminTokenAsync()
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = "admin-cli",
            ["username"] = _adminUser,
            ["password"] = _adminPasword,
        });

        var response = await http.PostAsync("/realms/master/protocol/openid-connect/token", form);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Keycloak admin token failed: {response.StatusCode} - {error}");
        }

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("access_token").GetString();
    }

    private async Task SetAuthHeaderAsync()
    {
        var token = await GetAdminTokenAsync();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<string> CreateUserAsync(string email, string password, string firstName, string lastName)
    {
        await SetAuthHeaderAsync();

        var body = JsonSerializer.Serialize(new
        {
            email,
            username = email,
            firstName,
            lastName,
            enabled = true,
            requiredActions = new [] { "VERIFY_EMAIL" },
            credentials = new[]
            {
                new { type = "password", value = password, temporary = false }
            }
        });

        var response = await http.PostAsync(
        $"/admin/realms/{_realm}/users",
            new StringContent(body, Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var userId = response.Headers.Location!.ToString().Split('/').Last();

        var redirectUri = Uri.EscapeDataString("http://localhost:3000/login");

        await http.PutAsync(
        $"/admin/realms/{_realm}/users/{userId}/send-verify-email?client_id=shoes-shop-client&redirect_uri={redirectUri}",
        null);
        return userId;
    }

    public async Task VerifyPasswordAsync(string email, string password)
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = "shoes-shop-client",
            ["username"] = email,
            ["password"] = password
        });

        var response = await http.PostAsync($"/realms/{_realm}/protocol/openid-connect/token", form);
        if (!response.IsSuccessStatusCode)
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }
    }

    public async Task UpdateUserAsync(string userId, string firstName, string lastName, string email)
    {
        await SetAuthHeaderAsync();

        var body = JsonSerializer.Serialize(new
        {
            firstName,
            lastName,
            email,
            username = email
        });

        var response = await http.PutAsync(
        $"/admin/realms/{_realm}/users/{userId}",
        new StringContent(body, Encoding.UTF8, "application/json")
        );

        response.EnsureSuccessStatusCode();
    }

    public async Task ChangePasswordAsync(string userId, string newPassword)
    {
        await SetAuthHeaderAsync();

        var body = JsonSerializer.Serialize(new
        {
            type = "password", value = newPassword, temporary = false
        });

        var response = await http.PutAsync(
        $"/admin/realms/{_realm}/users/{userId}/reset-password",
        new StringContent(body, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(string userId)
    {
        await SetAuthHeaderAsync();

        var response = await http.DeleteAsync($"/admin/realms/{_realm}/users/{userId}");
        response.EnsureSuccessStatusCode();
    }
}
