using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Chronicle.Managers;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Managers
{
    public class SagaSeekerTests
    {
        [Fact]
        public void Seek_Returns_SagaActions_For_Given_Message_Type()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient(typeof(ISagaAction<Message>), typeof(MySaga1));
            serviceCollection.AddTransient(typeof(ISagaAction<Message>), typeof(MySaga2));
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var seeker = new SagaSeeker(serviceProvider);

            var actions = seeker.Seek<Message>().ToList();

            actions.Count().ShouldBe(2);
            actions.First().GetType().ShouldBe(typeof(MySaga1));
            actions.Last().GetType().ShouldBe(typeof(MySaga2));
        }
    }

#region ARRANGE

    public class Message
    {
    }

    public class MySaga1 : Saga, ISagaStartAction<Message>
    {
        public Task HandleAsync(Message message, ISagaContext context)
        {
            throw new NotImplementedException();
        }

        public Task CompensateAsync(Message message, ISagaContext context)
        {
            throw new NotImplementedException();
        }
    }
    
    public class MySaga2 :Saga, ISagaStartAction<Message>
    {
        public Task HandleAsync(Message message, ISagaContext context)
        {
            throw new NotImplementedException();
        }

        public Task CompensateAsync(Message message, ISagaContext context)
        {
            throw new NotImplementedException();
        }
    }
        
#endregion
    
}
