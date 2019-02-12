using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle.Managers
{
    internal sealed class SagaSeeker : ISagaSeeker
    {
        private readonly IServiceProvider _serviceProvider;

        public SagaSeeker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public IEnumerable<ISagaAction<TMessage>> Seek<TMessage>()
            => _serviceProvider.GetService<IEnumerable<ISagaAction<TMessage>>>();
    }
}
