var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var catalogDb = postgres.AddDatabase("catalog-db");

var catalog = builder.AddProject<Projects.Catalog_Api>("catalog")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

var redis = builder.AddRedis("redis");

var keycloak = builder.AddKeycloak("keycloak", port: 8080)
    .WithDataVolume()
    .WithRealmImport("KeycloakConfig");

var basket = builder.AddProject<Projects.Basket_Api>("basket")
    .WithReference(redis)
    .WithReference(redis.GetEndpoint("tcp"))
    .WithReference(keycloak)
    .WaitFor(redis);

var orderDb = postgres.AddDatabase("order-db");

var order = builder.AddProject<Projects.Order_Api>("order")
    .WithReference(orderDb)
    .WithReference(keycloak)
    .WaitFor(orderDb);

var gateway = builder.AddProject<Projects.Gateway>("gateway")
    .WithReference(catalog)
    .WithReference(basket)
    .WithReference(order)
    .WaitFor(catalog)
    .WaitFor(basket)
    .WaitFor(order);

builder.Build().Run();
