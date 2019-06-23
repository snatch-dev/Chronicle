using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var t = _serviceProvider
                .GetService<IEnumerable<ISagaAction<TMessage>>>()
                .Union(_serviceProvider.GetService<IEnumerable<ISagaStartAction<TMessage>>>())
                .GroupBy(s => s.GetType())
                .Select(g => g.First())
                .Distinct();

            return t;
        }
    }
}
