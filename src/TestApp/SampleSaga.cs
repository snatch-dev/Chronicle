using System;
using System.Threading.Tasks;
using Chronicle;

namespace TestApp
{
    public class Message1
    {
        public string Text { get; set; }
    }

    public class Message2
    {
        public string Text { get; set; }
    }

    public class SagaData
    {
        public bool IsMessage1 { get; set; }
        public bool IsMessage2 { get; set; }
    }

    public class SampleSaga : Saga<SagaData>, ISagaStartAction<Message1>, ISagaAction<Message2>
    {

        public Task HandleAsync(Message2 message, ISagaContext context)
        {

            Data.IsMessage2 = true;
            Console.WriteLine("M2 reached!");
            CompleteSaga();
            return Task.CompletedTask;
        }

        public Task HandleAsync(Message1 message, ISagaContext context)
        {
            Data.IsMessage1 = true;
            Console.WriteLine("M1 reached!");
            CompleteSaga();
            return Task.CompletedTask;
        }

        public Task CompensateAsync(Message1 message, ISagaContext context)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"COMPANSATE M1 with message: {message.Text}");
            return Task.CompletedTask;
        }

        public Task CompensateAsync(Message2 message, ISagaContext context)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine($"COMPANSATE M2 with message: {message.Text}");

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
