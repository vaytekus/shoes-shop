using Basket.Api.Behaviors;
using Basket.Api.Exceptions;
using Basket.Api.Features.Basket;
using Basket.Api.Infrastructure;
using MediatR;
using Scalar.AspNetCore;
using FluentValidation;
using StackExchange.Redis;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapBasket();

app.Run();
