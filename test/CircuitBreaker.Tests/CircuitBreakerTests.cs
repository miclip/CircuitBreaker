using System;
using System.Threading;
using CircuitBreaker.Exceptions;
using Xunit;

namespace CircuitBreaker.Tests
{
    public class CircuitBreakerTests
    {
      private TimeSpan BreakerTimeout = new TimeSpan(0,0,10);
      private Action TimeoutAction =  () => 
      {
            Thread.Sleep(100);
            throw new TimeoutException("Operation Timed out");
      };

      private Action SuccessAction =  () => 
      {
            Thread.Sleep(100);
            Assert.True(true);
      };

      [Fact]
      public void OperationSuccess() 
      {
          var breaker = new CircuitBreaker();
          try
          {
            breaker.ExecuteAction(SuccessAction);
            Assert.True(true);
          }
          catch(CircuitBreakerOpenException openEx)
          {
             throw openEx;
          }
          catch (System.Exception ex)
          {
            throw ex;
          }
      }

      [Fact]
      public void OperationTimeoutThenSuccess() 
      {
          var breaker = new CircuitBreaker();
          try
          {
            breaker.ExecuteAction(TimeoutAction);
            Assert.True(true);
          }
          catch(CircuitBreakerOpenException openEx)
          {
             throw openEx;
          }
          catch (System.Exception ex)
          {
            Assert.NotNull(ex);
            breaker.ExecuteAction(SuccessAction);
          }
      }

      [Fact]
      public void OperationTimeoutThenIgnoreFailure() 
      {
          var breaker = new CircuitBreaker(BreakerTimeout);
          breaker.AddIgnoreExceptions(typeof(TimeoutException));
          try
          {
            breaker.ExecuteAction(TimeoutAction);
            Assert.True(true);
          }
          catch(CircuitBreakerOpenException openEx)
          {
             throw openEx;
          }
          catch (System.Exception ex)
          {
            Assert.NotNull(ex);
            Assert.True(breaker.IsClosed);            
          }
      }

       [Fact]
      public void OperationTimeoutThenFailure() 
      {
          var breaker = new CircuitBreaker(BreakerTimeout);
          try
          {
            breaker.ExecuteAction(TimeoutAction);
            Assert.True(true);
          }
          catch(CircuitBreakerOpenException openEx)
          {
             throw openEx;
          }
          catch (System.Exception ex)
          {
            Assert.NotNull(ex);
            Assert.True(breaker.IsOpen);
            if(breaker.IsClosed)
            {
              try
              {
                breaker.ExecuteAction(TimeoutAction);
                Assert.True(false,"Expects CircuitBreakerOpenException");
              }
              catch (CircuitBreakerOpenException openEx)
              {
                Assert.IsType<TimeoutException>(openEx.InnerException);
              }
            }
            
            
          }
      }
        
    }
}