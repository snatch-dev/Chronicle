using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chronicle.Builders;
using Chronicle.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Builders
{
    public class ChronicleBuilderTests
    {
        [Fact]
        public void UseInMemoryPersistence_Registers_InMemorySagaStateRepository_As_Singleton()
        {
            _builder.UseInMemoryPersistence();

            _services.ShouldContain(sd => 
                sd.ServiceType == typeof(ISagaStateRepository) && 
                sd.ImplementationType == typeof(InMemorySagaStateRepository) && 
                sd.Lifetime == ServiceLifetime.Singleton);
        }
        
        [Fact]
        public void UseInMemoryPersistence_Registers_InMemorySagaLog_As_Singleton()
        {
            _builder.UseInMemoryPersistence();

            _services.ShouldContain(sd => 
                sd.ServiceType == typeof(ISagaLog) && 
                sd.ImplementationType == typeof(InMemorySagaLog) && 
                sd.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void UseSagaLog_Registers_GivenImplementation_As_Transient()
        {
            _builder.UseSagaLog<MySagaLog>();
            
            _services.ShouldContain(sd => 
                sd.ServiceType == typeof(ISagaLog) && 
                sd.ImplementationType == typeof(MySagaLog) && 
                sd.Lifetime == ServiceLifetime.Transient);
        }
        
        [Fact]
        public void UseSagaStateRepository_Registers_GivenImplementation_As_Transient()
        {
            _builder.UseSagaStateRepository<MySagaStateRepository>();
            
            _services.ShouldContain(sd => 
                sd.ServiceType == typeof(ISagaStateRepository) && 
                sd.ImplementationType == typeof(MySagaStateRepository) && 
                sd.Lifetime == ServiceLifetime.Transient);
        }
        
        #region ARRANGE

        private readonly IServiceCollection _services;
        private readonly IChronicleBuilder _builder;
        
        public ChronicleBuilderTests()
        {
            _services = new ServiceCollection();
            _builder = new ChronicleBuilder(_services);    
        }
        
        public class MySagaLog : ISagaLog
        {
            public Task<IEnumerable<ISagaLogData>> ReadAsync(Guid id, Type type)
            {
                throw new NotImplementedException();
            }

            public Task WriteAsync(ISagaLogData message)
            {
                throw new NotImplementedException();
            }
        }
        
        public class MySagaStateRepository : ISagaStateRepository
        {
            public Task<ISagaState> ReadAsync(Guid id, Type type)
            {
                throw new NotImplementedException();
            }

            public Task WriteAsync(ISagaState state)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
