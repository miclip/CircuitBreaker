using System;

namespace CircuitBreaker
{
    public enum CircuitBreakerStateEnum
    {
       Open,
       HalfOpen,
       Closed
    }
}