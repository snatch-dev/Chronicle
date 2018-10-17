using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.Builders;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Chronicle.Tests.Builders
{
    public class ChronicleBuilderTests
    {

        #region ARRANGE
        private readonly IServiceCollection _services;
        private readonly IChronicleBuilder _builder;

        public ChronicleBuilderTests()
        {
            _services = Substitute.For<IServiceCollection>();
            _builder = new ChronicleBuilder(_services);
        }

        #endregion
    }
}
