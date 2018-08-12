using System;
using System.Threading.Tasks;
using Chronicle;

namespace TestApp
{
    public class Message1 { }
    public class Message2 { }

    public class SagaData
    {
        public bool IsMessage1 { get; set; }
        public bool IsMessage2 { get; set; }
    }

    public class SampleSaga : Saga<SagaData>, ISagaAsyncAction<Message1>, ISagaAsyncAction<Message2>
    {

        public Task HandleAsync(Message2 message)
        {
            Reject();
            return Task.CompletedTask;
        }

        public Task HandleAsync(Message1 message)
        {
            Data.IsMessage1 = true;
            CompleteSaga();
            return Task.CompletedTask;
        }

        public Task CompensateAsync(Message1 message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("COMPANSATE M1");
            return Task.CompletedTask;
        }

        public Task CompensateAsync(Message2 message)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("COMPANSATE M2");
            return Task.CompletedTask;
        }

        private void CompleteSaga()
        {
            if(Data.IsMessage1 && Data.IsMessage2)
            {
                Complete();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("SAGA COMPLETED");
            }
        }
    }
}
