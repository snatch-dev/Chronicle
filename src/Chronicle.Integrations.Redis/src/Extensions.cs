using System;
using Chronicle.Integrations.Redis.Persistence;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Chronicle.Integrations.Redis
{
    public static class Extensions
    {
        private static string DeserializationError => "Could not deserialize given appsettings.";

        public static IChronicleBuilder UseRedisPersistence(this IChronicleBuilder builder, string appSettingsSection, IConfiguration configuration)
        {
            ChronicleRedisSettings settings;
            try
            {
                settings = JsonConvert.DeserializeObject<ChronicleRedisSettings>(configuration.GetSection(appSettingsSection)?.Value);
            }
            catch
            {
                throw new ChronicleException(DeserializationError);
            }
            return builder.ConfigureRedisPersistence(settings);
        }

        public static IChronicleBuilder UseRedisPersistence(this IChronicleBuilder builder, ChronicleRedisSettings settings)
        {
            return builder.ConfigureRedisPersistence(settings);
        }

        private static IChronicleBuilder ConfigureRedisPersistence(this IChronicleBuilder builder, ChronicleRedisSettings settings)
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = settings.Configuration;
                options.InstanceName = settings.InstanceName;
            });
            builder.UseSagaLog<RedisSagaLog>();
            builder.UseSagaStateRepository<RedisSagaStateRepository>();

            return builder;
        }
    }
}