using AssetManager.Services;
using AssetManager.Sql;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Azure.Storage.Blobs;

namespace AssetManager;

public static class Dependencies
{
    public static IServiceCollection AddSqlDatabaseServices(IServiceCollection services, string connectionString, string dbName)
    {
        services.AddDbContext<AssetContext>(options => options.UseCosmos(connectionString, dbName));
        services.AddScoped<IAssetService, SqlAssetService>();
        return services;
    }

    public static IServiceCollection AddMongoDatabaseServices(IServiceCollection services, string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);
        services.AddSingleton(database);
        services.AddScoped<IAssetService, MongoAssetService>();
        return services;
    }

    public static IServiceCollection AddBlobStorageServices(IServiceCollection services, string connectionString, string dbName)
    {
        var containerClient = new BlobContainerClient(connectionString, dbName.ToLower());
        containerClient.CreateIfNotExists();
        services.AddSingleton(containerClient);
        services.AddScoped<IAssetService, BlobStorageAssetService>();
        return services;
    }
}