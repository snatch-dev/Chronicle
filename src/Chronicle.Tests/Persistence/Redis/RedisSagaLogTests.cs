using System;
using System.Linq;
using System.Threading.Tasks;
using Chronicle.Persistence;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Persistence
{
    public class RedisSagaLogTests
    {
        private readonly RedisSagaLog _redisSagaLog;
        private readonly IDistributedCache _cache;

        public RedisSagaLogTests()
        {
            _cache = Substitute.For<IDistributedCache>();
            _redisSagaLog = new RedisSagaLog(_cache);
        }

        [Fact]
        public async Task ReadAsync_Throws_ArgumentException_If_Id_Is_Null()
        {
            var exception = await Record.ExceptionAsync(() => _redisSagaLog.ReadAsync(null, Arg.Any<Type>()));

            exception.ShouldBeOfType<ArgumentException>();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task ReadAsync_Throws_ArgumentException_If_Id_Is_Whitespace()
        {
            var exception = await Record.ExceptionAsync(() => _redisSagaLog.ReadAsync("", Arg.Any<Type>()));

            exception.ShouldBeOfType<ArgumentException>();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task ReadAsync_Throws_ArgumentException_If_Type_Is_Null()
        {
            var exception = await Record.ExceptionAsync(() => _redisSagaLog.ReadAsync(Arg.Any<string>(), null));

            exception.ShouldBeOfType<ArgumentException>();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task WriteAsync_Throws_ArgumentException_If_Message_Is_Null()
        {
            var exception = await Record.ExceptionAsync(() => _redisSagaLog.WriteAsync(null));

            exception.ShouldBeOfType<ArgumentException>();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteAsync_Throws_ArgumentException_If_Id_Is_Whitespace()
        {
            var exception = await Record.ExceptionAsync(() => _redisSagaLog.DeleteAsync("", Arg.Any<Type>()));

            exception.ShouldBeOfType<ArgumentException>();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteAsync_Throws_ArgumentException_If_Type_Is_Null()
        {

            var exception = await Record.ExceptionAsync(() => _redisSagaLog.DeleteAsync(Arg.Any<string>(), null));

            exception.ShouldBeOfType<ArgumentException>();
            await Task.CompletedTask;
        }
    }
}