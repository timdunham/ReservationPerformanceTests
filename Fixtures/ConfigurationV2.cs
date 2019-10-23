using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReservationPerformanceTests.Extensions;
using Zoxive.HttpLoadTesting.Framework.Http;

namespace ReservationPerformanceTests.Fixtures
{
    public class ConfigurationV2: Configuration
    {
        internal override string StartConfigurationUrl => "api/v4/ui/start";
        internal override string ConfigureUrl => "api/v4/ui/configure";
        internal override string FinalizeConfigurationUrl => "api/v4/ui/finalize";
        internal override string CancelConfigurationUrl => "api/v4/ui/cancel";

        public ConfigurationV2(IUserLoadTestHttpClient userLoadTestHttpClient, string tenant, string rulesetNamespace, string ruleset)
            :base(userLoadTestHttpClient, tenant, rulesetNamespace, ruleset)
        {
        }

        internal override HttpContent GetInputParameters()
        {
            var inputParams = $@"{{
                ""Application"": {{ ""Instance"": ""{_tenant}"",""Name"": ""{_tenant}"" }},
                ""Part"": {{ ""Namespace"": ""{_rulesetNamespace}"", ""Name"": ""{_ruleset}""}},
                ""Mode"": 0,
                ""Profile"": ""default"",
                ""HeaderDetail"" : {{ ""HeaderId"": ""Simulator"", ""DetailId"": ""{Guid.NewGuid()}"" }},
                ""SourceHeaderDetail"" : {{ ""HeaderId"": """", ""DetailId"": """" }},
                ""VariantKey"" : """",
                ""IntegrationParameters"" : [{string.Join(',', _integrationParameters)}],
                ""RapidOptions"" : []
            }}";
            return new StringContent(inputParams, Encoding.UTF8, "application/json");
        }

        internal override HttpContent ChangeOption(string screenOptionCaption, string value)
        {
            var screenId = _ui.FindScreenId(screenOptionCaption);
            return new StringContent($@"{{
                ""sessionId"": ""{_ui.SessionId()}"",
                ""selections"": [
                    {{
                    ""ID"": ""{screenId}"",
                    ""Value"": ""{value}""
                    }}
                ]
            }}", Encoding.UTF8, "application/json");
        }

        internal override HttpContent GetSessionId()
        {
            return new StringContent(JsonConvert.SerializeObject(_ui.Value<string>("SessionID")), Encoding.UTF8, "application/json");
        }
    }
}
