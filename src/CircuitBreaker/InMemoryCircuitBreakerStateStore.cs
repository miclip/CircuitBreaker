using System;
using CircuitBreaker.Interfaces;

namespace CircuitBreaker
{
    public class InMemoryCircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private Exception _lastException;
        private DateTime _lastStateChanged;
        private CircuitBreakerStateEnum _state;
        private readonly object _lock = new object();

        bool ICircuitBreakerStateStore.IsClosed
        {
            get
            {
                return _state == CircuitBreakerStateEnum.Closed;
            }
        }

        Exception ICircuitBreakerStateStore.LastException
        {
            get
            {
                return _lastException;
            }
        }

        DateTime ICircuitBreakerStateStore.LastStateChangedDateUtc
        {
            get
            {
                return _lastStateChanged;
            }
        }

        CircuitBreakerStateEnum ICircuitBreakerStateStore.State
        {
            get
            {
                return _state;
            }
        }

        void ICircuitBreakerStateStore.HalfOpen()
        {
          lock(_lock)
          {
            _state = CircuitBreakerStateEnum.HalfOpen;
            _lastStateChanged = DateTime.UtcNow;
          }
        }

        void ICircuitBreakerStateStore.Reset()
        {
          lock(_lock)
          {
            _lastException = null;
            _lastStateChanged = DateTime.UtcNow;
            _state = CircuitBreakerStateEnum.Open;
          }
        }

        void ICircuitBreakerStateStore.Trip(Exception ex)
        {
          lock(_lock)
          {
            _lastException = ex;
            _state = CircuitBreakerStateEnum.Closed;
            _lastStateChanged = DateTime.UtcNow;
          }

        }
    }
}