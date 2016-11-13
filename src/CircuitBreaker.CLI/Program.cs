using System;
using System.IO;
using System.Net;
using System.Diagnostics.Tracing;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using CircuitBreaker.Exceptions;

namespace CircuitBreaker.CLI
{
    public class Program
    {
        private const string TestUrl = "http://reddit.com";
        public static void Main(string[] args)
        {
            var logger = SetupLogging();

            Action httpAction = ()=>{
             
             using (HttpClient client = new HttpClient())
             {
                client.Timeout = new TimeSpan(0,0,0,2);
                logger.LogInformation("Calling HTTP Endpoint: {0}", TestUrl);  
                var data = client.GetAsync(TestUrl).Result;
                var response = data.Content.ReadAsStringAsync().Result;
                logger.LogInformation("HTTP Status Code: ",data.StatusCode);                
              }
            };

            var breaker = new CircuitBreaker(new TimeSpan(0,0,30));
            while(true)
            {
                try
                {
                    breaker.ExecuteAction(httpAction);
                }
                catch (CircuitBreakerOpenException)
                {
                    logger.LogInformation("Half Open Exception caught, circuit is open: {0}", breaker.IsOpen);
                    Thread.Sleep(2000);
                }
                catch (System.Exception)
                {
                    logger.LogInformation("Exception caught, circuit is open: {0}", breaker.IsOpen);
                    Thread.Sleep(2000);
                }
                
            }
            

        }

        private static ILogger SetupLogging()
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();
            return loggerFactory.CreateLogger<Program>();            
        }
    }
}
