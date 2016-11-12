using System;
using Xunit;

namespace CircuitBreaker.Tests
{
    public class InMemoryCircuitBreakerStateStoreTests
    {
        [Fact]
        public void GetStore() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
        }

        [Fact]
        public void GetStore_IsOpen() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.False(store.IsClosed);
        }

        [Fact]
        public void GetStore_Trip() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.False(store.IsClosed);
            var tripException = new Exception("Force Trip of Circuit Breaker");
            var priorToTrip = DateTime.UtcNow;

            store.Trip(tripException);

            Assert.True(store.IsClosed);
            Assert.Equal(tripException,store.LastException);
            Assert.True(priorToTrip < store.LastStateChangedDateUtc);
        }

        [Fact]
        public void GetStore_HalfOpen() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.False(store.IsClosed);
            
            store.HalfOpen();

            Assert.False(store.IsClosed);
            
        }

        [Fact]
        public void GetStore_Reset() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.False(store.IsClosed);
            var tripException = new Exception("Force Trip of Circuit Breaker");
            var priorToTrip = DateTime.UtcNow;

            store.Trip(tripException);

            Assert.True(store.IsClosed);
            Assert.Equal(tripException,store.LastException);
            Assert.True(priorToTrip < store.LastStateChangedDateUtc);

            store.Reset();

            Assert.False(store.IsClosed);
        }
    }
}
