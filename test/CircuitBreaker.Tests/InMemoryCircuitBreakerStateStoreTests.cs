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
        public void GetStore_IsClosed() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.True(store.IsClosed);
        }

        [Fact]
        public void GetStore_Trip() 
        {
            var tripException = new Exception("Force Trip of Circuit Breaker");
            var priorToTrip = DateTime.UtcNow;
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.True(store.IsClosed);
            

            store.Trip(tripException);

            Assert.False(store.IsClosed);
            Assert.Equal(tripException,store.LastException);
            Assert.True(priorToTrip < store.LastStateChangedDateUtc);
        }

        [Fact]
        public void GetStore_HalfOpen() 
        {
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.True(store.IsClosed);
            
            store.HalfOpen();

            Assert.False(store.IsClosed);
            
        }

        [Fact]
        public void GetStore_Reset() 
        {
             var tripException = new Exception("Force Trip of Circuit Breaker");
            var priorToTrip = DateTime.UtcNow;
            var store = new InMemoryCircuitBreakerStateStore();
            Assert.NotNull(store);
            Assert.True(store.IsClosed);           

            store.Trip(tripException);

            Assert.False(store.IsClosed);
            Assert.Equal(tripException,store.LastException);
            Assert.True(priorToTrip < store.LastStateChangedDateUtc);

            store.Reset();

            Assert.True(store.IsClosed);
        }
    }
}
