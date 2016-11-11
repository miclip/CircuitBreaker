using System;

namespace CircuitBreaker.Interfaces
{
    public interface ICircuitBreakerStateStore
    {
        CircuitBreakerStateEnum State { get; }

        Exception LastException { get; }

        DateTime LastStateChangedDateUtc { get; }

        void Trip(Exception ex);

        void Reset();

        void HalfOpen();

        bool IsClosed { get; }
    }
}