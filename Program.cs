using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Infor.CPQ.Security.OAuth.Client.ZeroLegged;
using ReservationPerformanceTests.ReservationPerformanceTests;
using Zoxive.HttpLoadTesting.Client;
using Zoxive.HttpLoadTesting.Framework.Core;
using Zoxive.HttpLoadTesting.Framework.Core.Schedules;
using Zoxive.HttpLoadTesting.Framework.Model;

namespace ReservationPerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // Specify schedules. Add a few users run for a while and remove them. You can run any schedule in any order.
            // As long as you have active users!
            var schedule = new List<ISchedule>
            {    
                // Add Users over a period of time
                new AddUsers(totalUsers: 1, usersEvery: 1, seconds: 1),
                //new AddUsers(totalUsers: 30, usersEvery: 1, seconds: 15),
                // Run for a duration of time
                new Duration(15m),
                // Remove Users over a period of time
                new RemoveUsers(usersToRemove: 1, usersEvery:2, seconds: 1)
            };

            // Create as many tests as you want to run
            // These are the tests each User will run round robin style
            var tests = new List<ILoadTest>
            {
                new CreateReservation(),
                new UpdateLocationLocation()
            };

            var users = new List<IHttpUser>
            {
                new HttpUser("https://cfgdev.pcm.infor.com/", tests)
                {
                    AlterHttpClient = SetHttpClientProperties,
                    CreateHttpMessageHandler = SetHttpClientHandlerProperties,
                    AlterHttpRequestMessage = SetHttpRequestHeaders
                }
            };

            var testExecution = new LoadTestExecution(users);
            
            WebClient.Run(testExecution, schedule, null, args);
        }  

         private static void SetHttpClientProperties(HttpClient httpClient)
        {
            httpClient.Timeout = new TimeSpan(0, 1, 0);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

        private static HttpMessageHandler SetHttpClientHandlerProperties()
        {
            return new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate
            };
        }

        public static void SetHttpRequestHeaders(HttpRequestMessage request)
        {
            if (request.RequestUri.AbsoluteUri.Contains("DataImport"))
            {
                request.SetOAuthHeader(new ZeroLeggedHandlerOptions()
                {
                    ConsumerKey = "key",
                    ConsumerSecret = "secret",
                    SignatureMethod = Infor.CPQ.Security.OAuth.Common.SignatureMethod.Hmacsha256,
                    SupportNonceValidation = true,
                    SupportVersionValidation = true
                });
                request.Headers.Add("X-Infor-TenantId", "CPQ_DEV");
                request.Headers.Add("X-Infor-SecurityRoles", "CPQ-Designer");
            } 
            else 
            {
                request.Headers.Add("Authorization", "ApiKey D8C3928F106596A0C188A5522804ED");  //needed for v2 engine
                request.Headers.Add("ApiKey", "D8C3928F106596A0C188A5522804ED");                //needed for v1 engine
            }
        }
    }
}
