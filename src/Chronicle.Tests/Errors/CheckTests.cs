using Chronicle.Errors;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Errors
{
    public class CheckTests
    {
        [Fact]
        public void Is_Does_Nothing_If_Type_Is_Expected()
        {
            var type = typeof(CheckTests);

            var exception = Record.Exception(() => Check.Is<CheckTests>(type));

            exception.ShouldBeNull();
        }

        [Fact]
        public void Is_Throws_If_Type_Is_Not_Expected()
        {
            var type = typeof(CheckTests);

            var exception = Record.Exception(() => Check.Is<int>(type));

            exception.ShouldBeAssignableTo<ChronicleException>();
        }

        [Fact]
        public void IsNull_Does_Nothing_If_Data_Is_NotNull()
        {
            var exception = Record.Exception(() => Check.IsNull(new object()));

            exception.ShouldBeNull();
        }

        [Fact]
        public void IsNull_Throws_If_Data_Is_Null()
        {
            var exception = Record.Exception(() => Check.IsNull(default(CheckTests)));

            exception.ShouldBeAssignableTo<ChronicleException>();
        }
    }
}
