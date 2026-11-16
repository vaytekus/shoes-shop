using System.Text.Json;

namespace Order.Api.Infrastructure;

public class NovaPoshtaTrackingClient(HttpClient http, IConfiguration config)
{
    private readonly string _apiKey = config["NovaPoshta:ApiKey"]!;
    private readonly string _apiUrl = config["NovaPoshta:ApiUrl"]!;

    public async Task<string?> GetStatusCodeAsync(string trackingNumber, CancellationToken ct = default)
    {
        var payload = new
        {
            apiKey = _apiKey,
            modelName = "TrackingDocument",
            calledMethod = "getStatusDocuments",
            methodProperties = new
            {
                Documents = new[]
                {
                    new { DocumentNumber = trackingNumber }
                }
            }
        };

        var response = await http.PostAsJsonAsync(_apiUrl, payload, ct);
        response.EnsureSuccessStatusCode();

        using var doc = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        var data = doc.RootElement.GetProperty("data").EnumerateArray().FirstOrDefault();

        return data.ValueKind == JsonValueKind.Undefined
            ? null
            : data.GetProperty("StatusCode").GetString();
    }
}
