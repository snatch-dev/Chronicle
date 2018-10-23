using System;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle
{
    public interface IChronicleBuilder
    {
        IServiceCollection Services { get; }
        IChronicleBuilder UseInMemoryPersistence();
        IChronicleBuilder UseSagaLog(Type sagaLogType);
        IChronicleBuilder UseSagaDataRepository(Type repositoryType);
    }
}
