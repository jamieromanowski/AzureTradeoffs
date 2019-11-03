using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleCSHttpFunction.ABC.Pricing.Models;
using SampleCSHttpFunction.ABC.Pricing;

namespace ABC.Pricing
{
    public static class SimplePricingApiHttpCSharp
    {
        [FunctionName("SimplePricingApiHttpCSharp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                // add any logic for validating headers or authentication here
                PricingRequest pricingRequest = ParseRequest(req);

                if (pricingRequest == null)
                {
                    //throw error - request is invalid
                    return new BadRequestObjectResult("Invalid request");
                }

                PricingResponse result = PricingService.Calculate(pricingRequest, log);
                return (ActionResult)new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                log.LogCritical("unknown error occurred", req, ex.Message);
                return (ActionResult)new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        private static PricingRequest ParseRequest(HttpRequest httpRequest)
        {
            PricingRequest result = null;

            string requestBody = new StreamReader(httpRequest.Body).ReadToEndAsync().Result;
            dynamic requestObject = JsonConvert.DeserializeObject(requestBody);

            string userRole = requestObject?.UserRole;
            string modelNumber = requestObject?.ModelNumber;

            if(!String.IsNullOrEmpty(userRole) && !String.IsNullOrEmpty(modelNumber))
            {
                result = new PricingRequest { ModelNumber = modelNumber, UserRole = userRole };
            }

            return result;
        }
    }
}
