using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReservationPerformanceTests.Extensions;
using Zoxive.HttpLoadTesting.Framework.Http;
using Zoxive.HttpLoadTesting.Framework.Http.Json;

namespace ReservationPerformanceTests.Fixtures
{
    public class Configuration
    {
        private readonly IUserLoadTestHttpClient _userLoadTestHttpClient;
        private readonly string _tenant;
        private readonly string _rulesetNamespace;
        private readonly string _ruleset;
        private JToken _ui;
        private List<string> _integrationParameters = new List<string>();

        public Configuration(IUserLoadTestHttpClient userLoadTestHttpClient, string tenant, string rulesetNamespace, string ruleset)
        {
            _userLoadTestHttpClient = userLoadTestHttpClient;
            _tenant = tenant;
            _rulesetNamespace = rulesetNamespace;
            _ruleset = ruleset;
        }
        public Configuration WithIntegrationParameter(string name, string value, string dataType)
        {
            _integrationParameters.Add($"{{ \"Name\": \"{name}\", \"SimpleValue\": \"{value}\", \"isNull\": false, \"Type\": \"{dataType}\" }}");
            return this;
        }
        public async Task<Configuration> StartAsync()
        {
            var result =(await _userLoadTestHttpClient.Post("api/v4/ui/start", GetInputParameters(), null));
            _ui = result.AsJson();
            return this;
        }

        public async Task<Configuration> ConfigureAsync(string caption, string value, string stepName)
        {
            _ui = (await _userLoadTestHttpClient.Post($"api/v4/ui/configure?s={stepName}", _ui.ChangeOption(caption, value))).AsJson();
            return this;
        }
        
        public async Task<Configuration> Finalize()
        {
            var result = await _userLoadTestHttpClient.Post("api/v4/ui/finalize", new StringContent(JsonConvert.SerializeObject(_ui.SessionId()), Encoding.UTF8, "application/json"));
            return this;
        }

        public async Task<Configuration> Cancel()
        {
            var result = await _userLoadTestHttpClient.Post("api/v4/ui/cancel", new StringContent(JsonConvert.SerializeObject(_ui.SessionId()), Encoding.UTF8, "application/json"));
            return this;
        }

        private HttpContent GetInputParameters()
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
    ""RapidOptions"" : null
}}";
            return new StringContent( inputParams, Encoding.UTF8, "application/json");
        }
    }
}
