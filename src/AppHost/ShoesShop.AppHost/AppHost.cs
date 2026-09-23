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
    .WaitFor(redis);

builder.Build().Run();
