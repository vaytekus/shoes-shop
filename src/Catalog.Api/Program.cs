using System.Security.Claims;
using System.Text.Json.Serialization;
using Catalog.Api.Behaviors;
using Catalog.Api.Exceptions;
using Catalog.Api.Features.Categories;
using Catalog.Api.Features.Products;
using Catalog.Api.Features.Products.Commands.UploadProductImages;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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
                            var claims = new List<Claim>(jwt.Claims);
                            var realmAccess = jwt.Claims.FirstOrDefault(c => c.Type == "realm_access")?.Value;

                            if (realmAccess is not null)
                            {
                                var roles = System.Text.Json.JsonDocument.Parse(realmAccess)
                                    .RootElement
                                    .GetProperty("roles")
                                    .EnumerateArray()
                                    .Select(x => new Claim(ClaimTypes.Role, x.GetString()!));
                                claims.AddRange(roles);
                            }

                            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
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

builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("admin"));
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("catalog-db"))
        .UseSnakeCaseNamingConvention()
    );

builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.AddAzureBlobClient("azure-blobs");
builder.Services.AddScoped<BlobStorageService>();
builder.Services.AddScoped<UploadProductImagesValidator>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapProducts();
app.MapCategories();

app.Run();
