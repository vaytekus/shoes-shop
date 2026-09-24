var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var catalogDb = postgres.AddDatabase("catalog-db");

var catalog = builder.AddProject<Projects.Catalog_Api>("catalog")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

var redis = builder.AddRedis("redis");

var basket = builder.AddProject<Projects.Basket_Api>("basket")
    .WithReference(redis)
    .WithReference(redis.GetEndpoint("tcp"))
    .WaitFor(redis);

var orderDb = postgres.AddDatabase("order-db");

var order = builder.AddProject<Projects.Order_Api>("order")
    .WithReference(orderDb)
    .WaitFor(orderDb);

builder.Build().Run();
