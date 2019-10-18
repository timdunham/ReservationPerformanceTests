using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zoxive.HttpLoadTesting.Framework.Core;
using Zoxive.HttpLoadTesting.Framework.Http;
using Zoxive.HttpLoadTesting.Framework.Http.Json;

namespace ReservationPerformanceTests.ReservationPerformanceTests
{
    internal class CreateReservation : ILoadTest
    {
        public string Name => nameof(CreateReservation);

        public async Task Execute(IUserLoadTestHttpClient loadLoadTestHttpClient)
        {
            string sessionId="";
            try
            {
                var ui = (await loadLoadTestHttpClient.Post("api/v4/ui/start", GetInputParameters("CPQ_DEV", "Default","Reservation_v2"), null)).AsJson();
                sessionId=ui.SessionId();
                var ui2 = (await loadLoadTestHttpClient.Post("api/v4/ui/configure", ui.ChangeOption("ReservationLocation<font color=red>*</font>", "SJUT11ZE"))).AsJson();
                var ui3 = (await loadLoadTestHttpClient.Post("api/v4/ui/configure", ui2.ChangeOption("VehicleClass<font color=red>*</font>", "CCAR"))).AsJson();
                var finish = (await loadLoadTestHttpClient.Post("api/v4/ui/finalize", new StringContent(JsonConvert.SerializeObject(sessionId), Encoding.UTF8, "application/json")));
            }
            catch (System.Exception e)
            {
                var cancelResult = await loadLoadTestHttpClient.Post("api/v4/ui/cancel", new StringContent(JsonConvert.SerializeObject(sessionId), Encoding.UTF8, "application/json"));
                throw e; 
            }

        }
        
        public Task Initialize(ILoadTestHttpClient loadLoadTestHttpClient)
        {
             return Task.CompletedTask;
        }

        private HttpContent GetInputParameters(string tenant, string rulesetNamespace, string ruleset)
        {
            var inputParams = $@"{{
    ""Application"": {{ ""Instance"": ""{tenant}"",""Name"": ""{tenant}"" }},
    ""Part"": {{ ""Namespace"": ""{rulesetNamespace}"", ""Name"": ""{ruleset}""}},
    ""Mode"": 0,
    ""Profile"": ""default"",
    ""HeaderDetail"" : {{ ""HeaderId"": ""Simulator"", ""DetailId"": ""{Guid.NewGuid()}"" }},
    ""SourceHeaderDetail"" : {{ ""HeaderId"": """", ""DetailId"": """" }},
    ""VariantKey"" : """",
    ""IntegrationParameters"" : [
        {{ ""Name"": ""UseWorkspaceForSource"", ""SimpleValue"": ""true"", ""isNull"": false, ""Type"": ""boolean"" }},
        {{ ""Name"": ""UseWorkspaceForTarget"", ""SimpleValue"": ""true"", ""isNull"": false, ""Type"": ""boolean"" }},
        {{ ""Name"": ""SessionId"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CurrencyCode"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""ExchangeRate"", ""SimpleValue"": ""1"", ""isNull"": false, ""Type"": ""number"" }},
        {{ ""Name"": ""Company"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""Warehouse"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""ShipTo"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""ShipToState"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""BillTo"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""BillToState"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CustomerCompany"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CustomerLocation"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CustomerPO"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""RequestedDockDate"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""RequestedShipDate"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""RunMfgRules"", ""SimpleValue"": ""true"", ""isNull"": false, ""Type"": ""boolean"" }},
        {{ ""Name"": ""VariantKey"", ""SimpleValue"": """", ""isNull"": true, ""Type"": ""string"" }},
        {{ ""Name"": ""PartNumber"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CountryCode"", ""SimpleValue"": ""en-us"", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""DL_State"", ""SimpleValue"": ""OH"", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CancellationFlag"", ""SimpleValue"": ""false"", ""isNull"": false, ""Type"": ""boolean"" }},
        {{ ""Name"": ""CSA_Number"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""RAStatus"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""ReservationJSON"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""MarketSegment"", ""SimpleValue"": ""AUTO"", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""ReferralSourceCode"", ""SimpleValue"": ""ReferralSourceCode1"", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""CallingApp"", ""SimpleValue"": """", ""isNull"": false, ""Type"": ""string"" }},
        {{ ""Name"": ""Mode"", ""SimpleValue"": ""Reservation"", ""isNull"": false, ""Type"": ""string"" }}
    ],
    ""RapidOptions"" : null
}}";
            return new StringContent( inputParams, Encoding.UTF8, "application/json");
        }

      
    }
}