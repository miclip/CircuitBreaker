using System;
using CircuitBreaker.Interfaces;
using Xunit;

namespace CircuitBreaker.Tests
{
    public class CircuitBreakerStateStoreFactoryTests
    {
        [Fact]
        public void GetStoreFromFactory() 
        {
            var store = CircuitBreakerStateStoreFactory.GetCircuitBreakerStateStore();
            Assert.IsAssignableFrom<ICircuitBreakerStateStore>(store);
            Assert.NotNull(store);
        }

        
    }
}
