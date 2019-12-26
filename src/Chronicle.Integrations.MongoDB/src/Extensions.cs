using System;
using Chronicle.Integrations.MongoDB.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Chronicle.Integrations.MongoDB
{
    public static class Extensions
    {
        private static string DeserializationError => "Could not deserialize given appsettings.";

        public static IChronicleBuilder UseMongoPersistence(this IChronicleBuilder builder, string appSettingsSection)
        {
            return builder.UseMongoPersistence(GetDatabase);

            IMongoDatabase GetDatabase(IServiceProvider serviceProvider)
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                try
                {
                    var settings = JsonConvert.DeserializeObject<ChronicleMongoSettings>(configuration.GetSection(appSettingsSection)?.Value);
                    var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.Database);

                    return database;
                }
                catch
                {
                    throw new ChronicleException(DeserializationError);
                }
            }
        }

        public static IChronicleBuilder UseMongoPersistence(this IChronicleBuilder builder, ChronicleMongoSettings settings)
        {
            return builder.UseMongoPersistence(GetDatabase);

            IMongoDatabase GetDatabase(IServiceProvider serviceProvider)
                => new MongoClient(settings.ConnectionString).GetDatabase(settings.Database);
        }

        private static IChronicleBuilder UseMongoPersistence(this IChronicleBuilder builder, Func<IServiceProvider,IMongoDatabase> getDatabase)
        {
            builder.Services.AddTransient(getDatabase);
            builder.UseSagaLog<MongoSagaLog>();
            builder.UseSagaStateRepository<MongoSagaStateRepository>();

            return builder;
        }
    }
}
