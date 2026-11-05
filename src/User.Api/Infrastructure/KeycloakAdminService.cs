using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using User.Api.DTOs;

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

        var getResponse = await http.GetAsync($"/admin/realms/{_realm}/users/{userId}");
        getResponse.EnsureSuccessStatusCode();
        var existing = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        var attrs = new Dictionary<string, string[]>();
        if (existing.TryGetProperty("attributes", out var existingAttrs))
        {
            foreach (var prop in existingAttrs.EnumerateObject())
            {
                attrs[prop.Name] = prop.Value.EnumerateArray()
                    .Select(v => v.GetString() ?? "")
                    .ToArray();
            }
        }

        var body = JsonSerializer.Serialize(new
        {
            firstName,
            lastName,
            email,
            username = email,
            attributes = attrs
        });

        var response = await http.PutAsync(
            $"/admin/realms/{_realm}/users/{userId}",
            new StringContent(body, Encoding.UTF8, "application/json"));

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

    public async Task<JsonElement> GetUsersAsync(int first = 0, int max = 50, string? search = null)
    {
        await SetAuthHeaderAsync();

        var query = $"?first={first}&max={max}";
        if (!string.IsNullOrEmpty(search)) query += $"&search={Uri.EscapeDataString(search)}";

        var response = await http.GetAsync($"/admin/realms/{_realm}/users{query}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    public async Task<int> GetUsersCountAsync(string? search = null)
    {
        await SetAuthHeaderAsync();

        var query = search is not null ? $"?search={Uri.EscapeDataString(search)}" : "";
        var response = await http.GetAsync($"/admin/realms/{_realm}/users/count{query}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<(string AccessToken, string RefreshToken)> GetTokenAsync(string email, string password)
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password", ["client_id"] = "shoes-shop-client", ["username"] = email, ["password"] = password
        });

        var response = await http.PostAsync($"/realms/{_realm}/protocol/openid-connect/token", form);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            if (error.Contains("Account is not fully set up"))
            {
                throw new InvalidOperationException("Please verify your email before signing in");
            }

            throw new UnauthorizedAccessException($"Invalid credentials: {error}");
        }

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return (
            json.GetProperty("access_token").GetString()!,
            json.GetProperty("refresh_token").GetString()!
        );
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token", ["client_id"] = "shoes-shop-client", ["refresh_token"] = refreshToken
        });

        var response = await http.PostAsync($"/realms/{_realm}/protocol/openid-connect/token", form);

        if (!response.IsSuccessStatusCode)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return (
            json.GetProperty("access_token").GetString()!,
            json.GetProperty("refresh_token").GetString()!
        );
    }

    public async Task<JsonElement> GetUserByIdAsync(string userId)
    {
        await SetAuthHeaderAsync();

        var response = await http.GetAsync($"/admin/realms/{_realm}/users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    public async Task<UserProfileDto> GetMeAsync(string userId)
    {
        await SetAuthHeaderAsync();

        var response = await http.GetAsync($"/admin/realms/{_realm}/users/{userId}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        string? avatarUrl = null;

        if (json.TryGetProperty("attributes", out var attrs) &&
            attrs.TryGetProperty("avatarUrl", out var arr) &&
            arr.GetArrayLength() > 0)
        {
            avatarUrl = arr[0].GetString();
        }

        return new UserProfileDto(
            json.TryGetProperty("firstName", out var fn) ? fn.GetString() ?? "" : "",
            json.TryGetProperty("lastName", out var ln) ? ln.GetString() ?? "" : "",
            json.TryGetProperty("email", out var em) ? em.GetString() ?? "" : "",
            avatarUrl
        );
    }

    public async Task UpdateAvatarUrlAsync(string userId, string avatarUrl)
    {
        await SetAuthHeaderAsync();

        var getResponse = await http.GetAsync($"/admin/realms/{_realm}/users/{userId}");
        getResponse.EnsureSuccessStatusCode();
        var existing = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        var attrs = new Dictionary<string, string[]>();

        if (existing.TryGetProperty("attributes", out var existingAttrs))
        {
            foreach (var prop in existingAttrs.EnumerateObject())
            {
                attrs[prop.Name] = prop.Value.EnumerateArray()
                    .Select(v => v.GetString() ?? "")
                    .ToArray();
            }
        }

        attrs["avatarUrl"] = [avatarUrl];
        var body = JsonSerializer.Serialize(new
        {
            firstName = existing.TryGetProperty("firstName", out var fn) ? fn.GetString() : null,
            lastName = existing.TryGetProperty("lastName", out var ln) ? ln.GetString() : null,
            email = existing.TryGetProperty("email", out var em) ? em.GetString() : null,
            attributes = attrs
        });
        var putResponse = await http.PutAsync(
            $"/admin/realms/{_realm}/users/{userId}",
            new StringContent(body, Encoding.UTF8, "application/json"));
        putResponse.EnsureSuccessStatusCode();
    }
}
