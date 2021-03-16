using System;
using System.Linq;
using System.Threading.Tasks;
using Chronicle.Async;

namespace Chronicle.Managers
{
    internal sealed class SagaCoordinator : ISagaCoordinator
    {
        private readonly ISagaSeeker _seeker;
        private readonly ISagaInitializer _initializer;
        private readonly ISagaProcessor _processor;
        private readonly ISagaPostProcessor _postProcessor;
        private static readonly KeyedLocker Locker = new KeyedLocker();

        public SagaCoordinator(ISagaSeeker seeker, ISagaInitializer initializer, ISagaProcessor processor,
            ISagaPostProcessor postProcessor)
        {
            _seeker = seeker;
            _initializer = initializer;
            _processor = processor;
            _postProcessor = postProcessor;
        }

        public Task ProcessAsync<TMessage>(TMessage message, ISagaContext context = null) where TMessage : class
            => ProcessAsync(message: message, onCompleted: null, onRejected: null, context: context);

        public async Task ProcessAsync<TMessage>(TMessage message, Func<TMessage, ISagaContext, Task> onCompleted = null,
            Func<TMessage, ISagaContext, Task> onRejected = null, ISagaContext context = null) where TMessage : class
        {
            var actions = _seeker.Seek<TMessage>().ToList();

            Task EmptyHook(TMessage m, ISagaContext ctx) => Task.CompletedTask;
            onCompleted ??= EmptyHook;
            onRejected ??= EmptyHook;

            var sagaTasks = actions
                .Select(action => ProcessAsync(message, action, onCompleted, onRejected, context))
                .ToList();

            await Task.WhenAll(sagaTasks);
        }

        private async Task ProcessAsync<TMessage>(TMessage message, ISagaAction<TMessage> action,
            Func<TMessage, ISagaContext, Task> onCompleted, Func<TMessage, ISagaContext, Task> onRejected,
            ISagaContext context = null) where TMessage : class
        {
            context ??= SagaContext.Empty;
            var saga = (ISaga)action;
            var id = saga.ResolveId(message, context);

            using (await Locker.LockAsync(id))
            {
                var (isInitialized, state) = await _initializer.TryInitializeAsync(saga, id, message);

                if (!isInitialized)
                {
                    return;
                }

                await _processor.ProcessAsync(saga, message, state, context);
                await _postProcessor.ProcessAsync(saga, message, context, onCompleted, onRejected);
            }
        }
    }
}
