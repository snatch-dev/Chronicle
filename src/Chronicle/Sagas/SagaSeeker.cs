using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronicle.Sagas
{
    internal interface ISagaSeeker
    {
        IEnumerable<ISagaAction<TMessage>> Seek<TMessage>();
    }

    internal sealed class SagaSeeker : ISagaSeeker
    {
        private readonly IServiceProvider _serviceProvider;

        public SagaSeeker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public IEnumerable<ISagaAction<TMessage>> Seek<TMessage>()
        {
            var actions = (IEnumerable<ISagaAction<TMessage>>)_serviceProvider.GetService(typeof(IEnumerable<ISagaAction<TMessage>>));
            var startActions = (IEnumerable<ISagaAction<TMessage>>)_serviceProvider.GetService(typeof(IEnumerable<ISagaStartAction<TMessage>>));

            var result = actions.Concat(startActions);

            return result;
        }
    }
}
