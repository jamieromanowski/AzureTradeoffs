using DurableExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DurableExample
{
    public static class ChangePriceInSystem2
    {
        [FunctionName("ChangePriceInSystem2")]
        public static async Task<PriceChangeInSystemResponse> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            PriceChangeInSystemResponse result = new PriceChangeInSystemResponse { WasSuccessful = true };

            return await Task.FromResult(result);
        }
    }
}
