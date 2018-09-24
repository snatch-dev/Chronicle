using System.Collections.Generic;

namespace Chronicle.Managers
{
    internal interface ISagaSeeker
    {
        IEnumerable<ISagaAction<TMessage>> Seek<TMessage>();
    }
}
