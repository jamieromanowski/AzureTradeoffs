using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleCSHttpFunction.ABC.Pricing;
using SampleCSHttpFunction.ABC.Pricing.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebApp.Controllers
{
    [Route("api/[controller]")]
    public class PricingController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("still here");
        }
        // POST api/values
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post([Bind(include : "ModelNumber, UserRole")]PricingRequest pricingRequest)
        {
            try
            {

                if (pricingRequest == null)
                {
                    //throw error - request is invalid
                    return new BadRequestObjectResult("Invalid request");
                }

                PricingResponse result = PricingService.Calculate(pricingRequest);

                return (ActionResult)new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("It worked on my machine");
            }
        }
    }
}
