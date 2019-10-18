using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ReservationPerformanceTests.ReservationPerformanceTests
{
    public static class JTokenExtensions
    {  
        public static string FindScreenId(this JToken uiData, string screenOptionCaption)
        {
            var screen = uiData.SelectToken($"$...ScreenOptions[?(@.Caption=='{screenOptionCaption}')]");
            if (screen==null)
                throw new ApplicationException($"Unable to find page {screenOptionCaption}");
            return screen.Value<string>("ID");
        }
        
        public static string SessionId(this JToken uiData)
        {
            return uiData.Value<string>("SessionID");
        }

        public static HttpContent ChangeOption(this JToken uiData, string screenOptionCaption, string value)
        {
            var screenId = uiData.FindScreenId(screenOptionCaption);
            return new StringContent($@"{{
  ""SessionId"": ""{uiData.SessionId()}"",
  ""Selections"": [
    {{
      ""ID"": ""{screenId}"",
      ""Value"": ""{value}""
    }}
  ]
}}", Encoding.UTF8, "application/json");
        }
    }
}
