using System;
using CircuitBreaker.Interfaces;

namespace CircuitBreaker
{
   public class CircuitBreakerStateStoreFactory
   {
      public static ICircuitBreakerStateStore GetCircuitBreakerStateStore()
      {
         return new InMemoryCircuitBreakerStateStore();
      }
   }
}