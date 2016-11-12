using System;

namespace CircuitBreaker.Exceptions
{
  public class CircuitBreakerOpenException : Exception
  {
    private const string DefaultMessage = "Circuit Breaker is Open, action not attemped.";

    public CircuitBreakerOpenException() : base()
    {

    }
    public CircuitBreakerOpenException(Exception innerException): base(DefaultMessage, innerException) 
    {
      
    } 

    public CircuitBreakerOpenException(string message): base(message) 
    {
      
    } 
    public CircuitBreakerOpenException(string message, Exception innerException): base(message, innerException) 
    {
      
    } 

  }
}