using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableExample
{
    public static class TimerToCheckIfAnyRequestsExist
    {
        [FunctionName("TimerToCheckIfAnyRequestsExist")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, [OrchestrationClient]DurableOrchestrationClient starter, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Function input comes from the request content.
            Task instanceId = await starter.StartNewAsync("DurableProcessPriceChange", null);
            log.Info($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);

        }
    }
}
