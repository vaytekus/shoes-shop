using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;
using User.Api.Features.Users;
using User.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                if (authHeader?.StartsWith("Bearer ") == true)
                {
                    var token = authHeader["Bearer ".Length..];
                try
                {
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwt = handler.ReadJwtToken(token);
                        var identity = new ClaimsIdentity(jwt.Claims, JwtBearerDefaults.AuthenticationScheme);
                        context.Principal = new ClaimsPrincipal(identity);
                        context.Success();
                    }
                }
                catch { }
                }
                return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var keycloakUrl = builder.Configuration["services__keycloak__http__0"]
    ?? builder.Configuration["Keycloak:BaseUrl"]!;

builder.Services.AddHttpClient<KeycloakAdminService>(client => {
    client.BaseAddress = new Uri(keycloakUrl!);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapUsers();

app.Run();
