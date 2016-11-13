using System;
using System.Collections.Generic;
using CircuitBreaker.Interfaces;

namespace CircuitBreaker
{
    public class InMemoryCircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private Exception _lastException;
        private DateTime _lastStateChanged;
        private CircuitBreakerStateEnum _state = CircuitBreakerStateEnum.Closed;
        private readonly object _lockSync = new object();

        private readonly List<Type> _limitListExceptions = new List<Type>();
        private readonly List<Type> _ignoreListExceptions = new List<Type>();

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

        public void AddIgnoreExceptions(params Type[] exceptionTypes)
        {
            if(exceptionTypes==null)
             throw new ArgumentNullException("exceptionTypes");

            _ignoreListExceptions.AddRange(exceptionTypes);
        }

        public void RemoveIgnoreExceptions(params Type[] exceptionTypes)
        {
            if(exceptionTypes==null)
             throw new ArgumentNullException("exceptionTypes");
             
            foreach(var exceptionType in exceptionTypes)
            {
                _ignoreListExceptions.Remove(exceptionType);
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
            if(_ignoreListExceptions.Contains(ex.GetType()))
                return;

            lock(_lockSync)
            {
                _lastException = ex;
                _state = CircuitBreakerStateEnum.Open;
                _lastStateChanged = DateTime.UtcNow;
            }

        }
    }
}