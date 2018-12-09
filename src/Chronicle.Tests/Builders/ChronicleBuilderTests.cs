using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chronicle.Builders;
using Chronicle.Persistence;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Builders
{
    public class ChronicleBuilderTests
    {
        [Fact]
        public void UseInMemoryPersistence_Registers_In_Memory_Components()
        {
            _builder.UseInMemoryPersistence();

            _services.Received()
                .Add(Arg.Is<ServiceDescriptor>(sd =>
                    sd.Lifetime == ServiceLifetime.Singleton &&
                    sd.ServiceType == typeof(ISagaDataRepository) &&
                    sd.ImplementationType == typeof(InMemorySagaDataRepository)));

            _services.Received()
                .Add(Arg.Is<ServiceDescriptor>(sd =>
                    sd.Lifetime == ServiceLifetime.Singleton &&
                    sd.ServiceType == typeof(ISagaLog) &&
                    sd.ImplementationType == typeof(InMemorySagaLog)));
        }

        [Fact]
        public void UseSagaLog_Throws_If_SagaLogType_Does_Not_Assign_To_ISagaLog()
        {
            var exception = Record.Exception(() => _builder.UseSagaLog(typeof(object)));

            exception.ShouldBeAssignableTo<ChronicleException>();
        }

        [Fact]
        public void UseSagaLog_Registers_If_SagaLogType_Assigns_To_ISagaLog()
        {
            var type = typeof(CustomSagaLog);

            _builder.UseSagaLog(type);

            _services.Received()
                .Add(Arg.Is<ServiceDescriptor>(sd =>
                    sd.Lifetime == ServiceLifetime.Transient &&
                    sd.ServiceType == typeof(ISagaLog) &&
                    sd.ImplementationType == type));
        }

        [Fact]
        public void UseSagaDataRepository_Throws_If_RepositoryType_Does_Not_Assign_To_ISagaDataRepository()
        {
            var exception = Record.Exception(() => _builder.UseSagaDataRepository(typeof(object)));

            exception.ShouldBeAssignableTo<ChronicleException>();
        }

        [Fact]
        public void UseSagaDataRepository_Registers_If_RepositoryType_Assigns_To_ISagaDataRepository()
        {
            var type = typeof(CustomSagaDataRepository);

            _builder.UseSagaDataRepository(type);

            _services.Received()
                .Add(Arg.Is<ServiceDescriptor>(sd =>
                    sd.Lifetime == ServiceLifetime.Transient &&
                    sd.ServiceType == typeof(ISagaDataRepository) &&
                    sd.ImplementationType == type));
        }

        #region ARRANGE
        private readonly IServiceCollection _services;
        private readonly IChronicleBuilder _builder;

        public class CustomSagaLog : ISagaLog
        {
            public Task<IEnumerable<ISagaLogData>> GetAsync(Guid sagaId, Type sagaType)
            {
                throw new NotImplementedException();
            }

            public Task SaveAsync(ISagaLogData message)
            {
                throw new NotImplementedException();
            }
        }

        public class CustomSagaDataRepository : ISagaDataRepository
        {
            public Task<ISagaData> ReadAsync(Guid sagaId, Type sagaType)
            {
                throw new NotImplementedException();
            }

            public Task WriteAsync(ISagaData sagaData)
            {
                throw new NotImplementedException();
            }
        }

        public ChronicleBuilderTests()
        {
            _services = Substitute.For<ServiceCollection>();
            _builder = new ChronicleBuilder(_services);
        }

        #endregion
    }
}
