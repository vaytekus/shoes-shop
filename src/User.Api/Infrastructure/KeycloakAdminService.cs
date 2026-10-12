namespace User.Api.Infrastructure;

public class KeycloakAdminService(HttpClient http, IConfiguration config)
{
    private readonly string _realm = config["Keycloak:Realm"] ?? "shoes-shop";
    private readonly string _adminUser = config["Keycloak:AdminUsername"] ?? "admin";
    private readonly string _adminPasword = config["Keycloak:AdminPassword"] ?? "shoes-shop";

    public async Task<>
}
