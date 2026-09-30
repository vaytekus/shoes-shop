using System.Security.Claims;
using System.Text.Json.Serialization;
using Basket.Api.Behaviors;
using Basket.Api.Exceptions;
using Basket.Api.Features.Basket;
using Basket.Api.Infrastructure;
using MediatR;
using Scalar.AspNetCore;
using FluentValidation;
using StackExchange.Redis;
using System.Text.RegularExpressions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Basket.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Logging.AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug);

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

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    // services__redis__tcp__0 = "tcp://localhost:PORT" — plain endpoint без SSL
    var tcpUrl = builder.Configuration["services__redis__tcp__0"];

    if (builder.Environment.IsDevelopment() && tcpUrl != null)
    {
        var hostPort = tcpUrl.Replace("tcp://", "");
        var sslCs = builder.Configuration.GetConnectionString("redis") ?? "";
        var match = System.Text.RegularExpressions.Regex.Match(sslCs, @"password=([^,]+)");
        var cs = match.Success ? $"{hostPort},password={match.Groups[1].Value}" : hostPort;
        return ConnectionMultiplexer.Connect(cs);
    }

    // Production: Azure Redis — ssl=true залишається
    var opts = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("redis")!);
    return ConnectionMultiplexer.Connect(opts);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddMassTransit(x => {
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapBasket();

app.Run();
