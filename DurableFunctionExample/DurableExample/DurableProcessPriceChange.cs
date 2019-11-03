using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DurableExample.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableExample
{
    public static class DurableProcessPriceChange
    {
        [FunctionName("DurableProcessPriceChange")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            List<string> outputs = new List<string>();

            // get request
            PriceChangeRequest request = context.GetInput<PriceChangeRequest>();

            // wait until the start time if needed
            if (request.EffectiveDate == null && request.EffectiveDate > DateTime.UtcNow)
            {
                await context.CreateTimer(request.EffectiveDate??DateTime.UtcNow, CancellationToken.None);
            }

            // process for all of the systems that need the price changed in them
            var parallelTasks = new List<Task<PriceChangeInSystemResponse>>();

            // you could do this in an orchestrator loop, but I am simplifying this here

            Task<PriceChangeInSystemResponse> system1Task = context.CallActivityAsync<PriceChangeInSystemResponse>("ChangePriceInSystem1", request);
            parallelTasks.Add(system1Task);

            Task<PriceChangeInSystemResponse> system2Task = context.CallActivityAsync<PriceChangeInSystemResponse>("ChangePriceInSystem2", request);
            parallelTasks.Add(system2Task);

            // Clean up
            await Task.WhenAll(parallelTasks);

            foreach (PriceChangeInSystemResponse result in parallelTasks
                .Where(_ => !_.Result.WasSuccessful)
                .Select(_ => _.Result))
            {
                outputs.Add(result.Message);
            }

            //done

            return outputs;
        }

        [FunctionName("DurableProcessPriceChange_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("DurableProcessPriceChange_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableProcessPriceChange", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}