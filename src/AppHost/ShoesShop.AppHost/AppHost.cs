var builder = DistributedApplication.CreateBuilder(args);

var azureBlobs = builder.AddConnectionString("azure-blobs");

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin(c => c.WithHostPort(5050));

var catalogDb = postgres.AddDatabase("catalog-db");

var catalog = builder.AddProject<Projects.Catalog_Api>("catalog")
    .WithReference(catalogDb)
    .WithReference(azureBlobs)
    .WaitFor(catalogDb);

var redis = builder.AddRedis("redis")
    .WithDataVolume()
    .WithHostPort(6379);

var keycloak = builder.AddKeycloak("keycloak", port: 8080)
    .WithDataVolume()
    .WithRealmImport("KeycloakConfig");

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var basket = builder.AddProject<Projects.Basket_Api>("basket")
    .WithReference(redis)
    .WithReference(redis.GetEndpoint("tcp"))
    .WithReference(keycloak)
    .WithReference(rabbitmq)
    .WaitFor(redis)
    .WaitFor(rabbitmq);

var orderDb = postgres.AddDatabase("order-db");

var order = builder.AddProject<Projects.Order_Api>("order")
    .WithReference(orderDb)
    .WithReference(keycloak)
    .WithReference(rabbitmq)
    .WaitFor(orderDb)
    .WaitFor(rabbitmq);

var user = builder.AddProject<Projects.User_Api>("user")
    .WithReference(keycloak)
    .WaitFor(keycloak);

var gateway = builder.AddProject<Projects.Gateway>("gateway")
    .WithReference(catalog)
    .WithReference(basket)
    .WithReference(order)
    .WithReference(user)
    .WaitFor(catalog)
    .WaitFor(basket)
    .WaitFor(order)
    .WaitFor(user);

builder.Build().Run();
