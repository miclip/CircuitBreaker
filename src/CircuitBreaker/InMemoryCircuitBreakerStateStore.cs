using System;
using CircuitBreaker.Interfaces;

namespace CircuitBreaker
{
    public class InMemoryCircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private Exception _lastException;
        private DateTime _lastStateChanged;
        private CircuitBreakerStateEnum _state = CircuitBreakerStateEnum.Closed;
        private readonly object _lockSync = new object();

        public bool IsClosed
        {
            get
            {
                return _state == CircuitBreakerStateEnum.Closed;
            }
        }

        public Exception LastException
        {
            get
            {
                return _lastException;
            }
        }

        public DateTime LastStateChangedDateUtc
        {
            get
            {
                return _lastStateChanged;
            }
        }

        public CircuitBreakerStateEnum State
        {
            get
            {
                return _state;
            }
        }

        public void HalfOpen()
        {
          lock(_lockSync)
          {
            _state = CircuitBreakerStateEnum.HalfOpen;
            _lastStateChanged = DateTime.UtcNow;
          }
        }

        public void Reset()
        {
          lock(_lockSync)
          {
            _lastException = null;
            _lastStateChanged = DateTime.UtcNow;
            _state = CircuitBreakerStateEnum.Closed;
          }
        }

        public void Trip(Exception ex)
        {
          lock(_lockSync)
          {
            _lastException = ex;
            _state = CircuitBreakerStateEnum.Open;
            _lastStateChanged = DateTime.UtcNow;
          }

        }
    }
}