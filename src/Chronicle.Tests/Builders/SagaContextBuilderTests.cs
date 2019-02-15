using System;
using System.Linq;
using Chronicle.Builders;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Builders
{
    public class SagaContextBuilderTests
    {
        [Fact]
        public void WithCorrelationId_Sets_CorrelationId_Field_With_Given_Data()
        {
            var correlationId = Guid.NewGuid();

            var context = _builder
                .WithCorrelationId(correlationId)
                .Build();

            context.CorrelationId.ShouldBe(correlationId);
        }
        
        [Fact]
        public void WithOriginator_Sets_Originator_Field_With_Given_Data()
        {
            var originator = "originator";

            var context = _builder
                .WithOriginator(originator)
                .Build();

            context.Originator.ShouldBe(originator);
        }
        
        [Fact]
        public void WithMetadata_Adds_SagaContextMetadata_To_List_With_Given_Values()
        {
            var key = "key";
            var value = "value";

            var context = _builder
                .WithMetadata(key, value)
                .Build();

            context.Metadata.Count.ShouldBe(1);
            context.Metadata.First().Key.ShouldBe(key);
            context.Metadata.First().Value.ShouldBe(value);
        }
        
        
#region ARRANGE

        private readonly ISagaContextBuilder _builder;
        
        public SagaContextBuilderTests()
        {
            _builder = new SagaContextBuilder();    
        }
        
#endregion
    }
}
