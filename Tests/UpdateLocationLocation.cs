using ReservationPerformanceTests.Fixtures;
using System.Threading.Tasks;
using Zoxive.HttpLoadTesting.Framework.Core;
using Zoxive.HttpLoadTesting.Framework.Http;

namespace ReservationPerformanceTests.ReservationPerformanceTests
{
    public class UpdateLocationLocation : ILoadTest
    {
        public string Name => nameof(UpdateLocationLocation);

        public async Task Execute(IUserLoadTestHttpClient loadLoadTestHttpClient)
        {
            var result = new DataImport(loadLoadTestHttpClient, "CPQ_DEV", "AccountAccount")
                .WithValue("AccountNumber", "100013")
                .WithValue("CountryCode", "US-1")
                .UpdateRow("100013", 1);
            await result;
        }

        public Task Initialize(ILoadTestHttpClient loadLoadTestHttpClient)
        {
            return Task.CompletedTask;
        }
    }
}