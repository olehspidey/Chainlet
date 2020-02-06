using System;
using System.Threading.Tasks;
using Chainlet.Chains;
using Chainlet.Tests.TestFixtures;
using Xunit;

namespace Chainlet.Tests
{
    public class SimplePipeTest : IClassFixture<SimpleTestFixture<int>>
    {
        private readonly SimpleTestFixture<int> _simpleTestFixture;
        public SimplePipeTest(SimpleTestFixture<int> simpleTestFixture)
        {
            _simpleTestFixture = simpleTestFixture;
        }

        [Fact]
        public void Empty_Prop_Should_Return_New_Instance()
        {
            Assert.NotNull(SimpleChain<int>.Empty);
        }

        [Fact]
        public async Task Should_Arrange_Next_Handlers()
        {
            await SimpleChain<int>
                .Empty
                .Pin(_simpleTestFixture.HandlerFirst)
                .Pin(_simpleTestFixture.HandlerSecond)
                .ProcessAsync(0);

            Assert.Equal(_simpleTestFixture.HandlerSecond, _simpleTestFixture.HandlerFirst.Next);
            Assert.Null(_simpleTestFixture.HandlerSecond.Next);
        }

        [Fact]
        public void Should_Throw_Exception_If_Handler_Null()
         => Assert.Throws<ArgumentNullException>(() => SimpleChain<int>
             .Empty
             .Pin(null));
    }
}
