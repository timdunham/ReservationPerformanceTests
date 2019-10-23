using System;
using System.Net.Http;
using System.Text;
using Zoxive.HttpLoadTesting.Framework.Http;

namespace ReservationPerformanceTests.Fixtures
{
    public class ConfigurationV1 : Configuration
    {
        internal override string StartConfigurationUrl => "ConfiguratorService/v3/ProductConfiguratorUI.svc/json/StartConfiguration";
        internal override string ConfigureUrl => "ConfiguratorService/v3/ProductConfiguratorUI.svc/json/Configure";
        internal override string FinalizeConfigurationUrl => "ConfiguratorService/v3/ProductConfiguratorUI.svc/json/FinalizeConfiguration";
        internal override string CancelConfigurationUrl => "ConfiguratorService/v3/ProductConfiguratorUI.svc/json/CancelConfiguration";

        public ConfigurationV1(IUserLoadTestHttpClient userLoadTestHttpClient, string tenant, string rulesetNamespace, string ruleset)
            : base(userLoadTestHttpClient, tenant, rulesetNamespace,  ruleset)
        {        
        }
        
        internal override HttpContent GetInputParameters()
        {
            var inputParams = $@"{{ ""inputParameters"" : {{
                ""Application"": {{ ""Instance"": ""{_tenant}"",""Name"": ""{_tenant}"",""User"": ""test"" }},
                ""Part"": {{ ""Namespace"": ""{_rulesetNamespace}"", ""Name"": ""{_ruleset}""}},
                ""Mode"": 0,
                ""Profile"": ""default"",
                ""HeaderDetail"" : {{ ""HeaderId"": ""Simulator"", ""DetailId"": ""{Guid.NewGuid()}"" }},
                ""SourceHeaderDetail"" : {{ ""HeaderId"": """", ""DetailId"": """" }},
                ""VariantKey"" : null,
                ""IntegrationParameters"" : [{string.Join(',', _integrationParameters)}],
                ""RapidOptions"" : []
            }} }}";
            return new StringContent( inputParams, Encoding.UTF8, "application/json");
        }

        internal override HttpContent ChangeOption(string screenOptionCaption, string value)
        {
            var screenId = FindScreenId(screenOptionCaption);
            return new StringContent($@"{{
                ""sessionID"": ""{SessionId()}"",
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
            return new StringContent($@"{{ ""sessionID"": ""{SessionId()}"" }}", Encoding.UTF8, "application/json");
        }
    }
}
