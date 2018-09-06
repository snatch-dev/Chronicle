using System;

namespace Chronicle
{
    public interface IChronicleBuilder
    {
        IChronicleBuilder UseInMemoryPersistence();
        IChronicleBuilder UseSagaLog(Type sagaLogType);
        IChronicleBuilder UseSagaDataRepository(Type repositoryType);
    }
}
