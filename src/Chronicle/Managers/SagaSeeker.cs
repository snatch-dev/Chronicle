using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle.Managers
{
    internal sealed class SagaSeeker : ISagaSeeker
    {
        private readonly IServiceProvider _serviceProvider;

        public SagaSeeker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public IEnumerable<ISagaAction<TMessage>> Seek<TMessage>()
        {
            var actions = _serviceProvider.GetService<IEnumerable<ISagaAction<TMessage>>>();
            var startActions = _serviceProvider.GetService<IEnumerable<ISagaStartAction<TMessage>>>();

            var result = actions
                .Concat(startActions)
                .GroupBy(a => a.GetType())
                .Select(g => g.First());

            return result;
        }
    }
}
