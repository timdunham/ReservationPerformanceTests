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
    public abstract class Configuration
    {
        internal readonly IUserLoadTestHttpClient _userLoadTestHttpClient;
        internal readonly string _tenant;
        internal readonly string _rulesetNamespace;
        internal readonly string _ruleset;
        internal JToken _ui;
        internal List<string> _integrationParameters = new List<string>();
        internal abstract string StartConfigurationUrl {get;}
        internal abstract string ConfigureUrl {get;}
        internal abstract string FinalizeConfigurationUrl {get;}
        internal abstract string CancelConfigurationUrl {get;}
        internal abstract HttpContent GetSessionId();
        internal abstract HttpContent GetInputParameters();
        internal abstract HttpContent ChangeOption(string screenOptionCaption, string value);

        public Configuration(IUserLoadTestHttpClient userLoadTestHttpClient, string tenant, string rulesetNamespace, string ruleset)
        {
            _userLoadTestHttpClient = userLoadTestHttpClient;
            _tenant = tenant;
            _rulesetNamespace = rulesetNamespace;
            _ruleset = ruleset;
        }
        public Configuration WithIntegrationParameter(string name, string value, string dataType)
        {
            var dataTypeNumber = (dataType=="number")? 1 : (dataType=="boolean") ? 2 : 0;
                
            _integrationParameters.Add($"{{ \"Name\": \"{name}\", \"SimpleValue\": \"{value}\", \"IsNull\": false, \"Type\": \"{dataTypeNumber}\" }}"); //isNull vs IsNull?
            return this;
        }
        public async Task<Configuration> StartAsync()
        {
            var result =(await _userLoadTestHttpClient.Post(StartConfigurationUrl, GetInputParameters(), null));
            _ui = result.AsJson();
            return this;
        }

        public async Task<Configuration> ConfigureAsync(string caption, string value, string stepName)
        {
            _ui = (await _userLoadTestHttpClient.Post(ConfigureUrl, ChangeOption(caption, value))).AsJson();
            return this;
        }
        
        public async Task<Configuration> Finalize()
        {
            var result = await _userLoadTestHttpClient.Post(FinalizeConfigurationUrl, GetSessionId());
            return this;
        }

        public async Task<Configuration> Cancel()
        {
            var result = await _userLoadTestHttpClient.Post(CancelConfigurationUrl, GetSessionId());
            return this;
        }

        internal virtual string FindScreenId(string screenOptionCaption)
        {
            var screen = _ui.SelectToken($"$...ScreenOptions[?(@.Caption=='{screenOptionCaption}')]");
            if (screen==null)
                throw new ApplicationException($"Unable to find page {screenOptionCaption}");
            return screen.Value<string>("ID");
        }
        
    }
}