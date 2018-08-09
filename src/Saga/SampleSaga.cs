using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class Message1 { }
    public class Message2 { }

    public class SampleState : ISagaData
    {
        public bool IsMessage1 { get; set; }
        public bool IsMessage2 { get; set; }
        public Guid Id { get; set; }
        public SagaStates State { get; set; }
    }

    public class SampleSaga : BaseSaga<SampleState>, ISagaAction<Message1>, ISagaAction<Message2>
    {
        public SampleSaga(ISagaDataRepository<SampleState> repository)
            : base(repository)

        {
        }

        public async Task HandleAsync(Message2 message)
        {
            Data.IsMessage1 = true;
            await CompleteSagaAsync();
        }

        public async Task HandleAsync(Message1 message)
        {
            Data.IsMessage2 = true;
            await CompleteSagaAsync();
        }

        public Task CompensateAsync(Message2 message)
        {
            throw new NotImplementedException();
        }

        public Task CompensateAsync(Message1 message)
        {
            throw new NotImplementedException();
        }

        private async Task CompleteSagaAsync()
        {
            if(Data.IsMessage1 && Data.IsMessage2)
            {
                await CompleteAsync();
            }
        }
    }
}
