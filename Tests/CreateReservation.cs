using System.Threading.Tasks;
using ReservationPerformanceTests.Fixtures;
using Zoxive.HttpLoadTesting.Framework.Core;
using Zoxive.HttpLoadTesting.Framework.Http;

namespace ReservationPerformanceTests.ReservationPerformanceTests
{
    internal class CreateReservation : ILoadTest
    {
        public string Name => nameof(CreateReservation);

        public async Task Execute(IUserLoadTestHttpClient loadLoadTestHttpClient)
        {
            var test = new Configuration(loadLoadTestHttpClient, "CPQ_DEV", "Default", "Reservation_v2")
                .WithIntegrationParameter("ReservationJSON","","string")
                .WithIntegrationParameter("ReferralSourceCode", "", "string")
                .WithIntegrationParameter("CallingApp", "", "string")
                .WithIntegrationParameter("Mode", "Reservation", "string")
                .WithIntegrationParameter("CurrencyCode", "", "string")
                .WithIntegrationParameter("ExchangeRate", "1", "number");
            
            try
            {
                await test.StartAsync();
                await test.ConfigureAsync("ReservationLocation<font color=red>*</font>", "SJUT11ZE", "Location");
                await test.ConfigureAsync("VehicleClass<font color=red>*</font>", "CCAR", "Vehicle");    
                await test.Finalize();
            }
            catch (System.Exception)
            {
                await test.Cancel();
                //throw e; 
            }

        }
        
        public Task Initialize(ILoadTestHttpClient loadLoadTestHttpClient)
        {
             return Task.CompletedTask;
        }

        // {{ ""Name"": ""UseWorkspaceForSource"", ""SimpleValue"": ""true"", ""isNull"": false, ""Type"": ""boolean"" }},
        // {{ ""Name"": ""UseWorkspaceForTarget"", ""SimpleValue"": ""true"", ""isNull"": false, ""Type"": ""boolean"" }},
        // {{ ""Name"": ""SessionId"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CurrencyCode"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""ExchangeRate"", ""SimpleValue"": ""1"", ""isNull"": false, ""Type"": ""number"" }},
        // {{ ""Name"": ""Company"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""Warehouse"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""ShipTo"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""ShipToState"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""BillTo"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""BillToState"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CustomerCompany"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CustomerLocation"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CustomerPO"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""RequestedDockDate"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""RequestedShipDate"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""RunMfgRules"", ""SimpleValue"": ""true"", ""isNull"": false, ""Type"": ""boolean"" }},
        // {{ ""Name"": ""VariantKey"", ""SimpleValue"": """", ""isNull"": true, ""Type"": ""string"" }},
        // {{ ""Name"": ""PartNumber"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CountryCode"", ""SimpleValue"": ""en-us"", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""DL_State"", ""SimpleValue"": ""OH"", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CancellationFlag"", ""SimpleValue"": ""false"", ""isNull"": false, ""Type"": ""boolean"" }},
        // {{ ""Name"": ""CSA_Number"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""RAStatus"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""ReservationJSON"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""MarketSegment"", ""SimpleValue"": ""AUTO"", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""ReferralSourceCode"", ""SimpleValue"": ""ReferralSourceCode1"", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""CallingApp"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        // {{ ""Name"": ""Mode"", ""SimpleValue"": ""Reservation"", ""isNull"": false, ""Type"": ""string"" }}
    
        

      
    }
}