using Microsoft.Extensions.Logging;
using SampleCSHttpFunction.ABC.Pricing.Models;
using System;

namespace SampleCSHttpFunction.ABC.Pricing
{
    class PricingService
    {
        // Please note, this is written in a style to optimize its ability to read and grok quickly
        // Please do not write your production code like this

        public static PricingResponse Calculate(PricingRequest request)
        {
            if(String.IsNullOrEmpty(request.UserRole))
            {
                return CreateErrorResponse("User does not have a role in system.", request.ModelNumber);
            }

            if (String.IsNullOrEmpty(request.UserRole))
            {
                return CreateErrorResponse("User is not able to order product.", request.ModelNumber);
            }

            try
            {
                PricingResponse response = new PricingResponse { ModelNumber = request.ModelNumber };
                response.ListPrice = GetListPrice(request.ModelNumber);
                response.UserPrice = GetUserPrice(response.ListPrice, request.UserRole);

                return response;
            }
            catch(Exception ex)
            {
                // return vague user friendly message
                return CreateErrorResponse("That's weird - it worked on my machine, let me look at it", request.ModelNumber);
            }

        }

        private static PricingResponse CreateErrorResponse(string errorReason, string modelNumber)
        {
            return new PricingResponse
            {
                ErrorReason = errorReason,
                ListPrice = null,
                UserPrice = null,
                ModelNumber = modelNumber
            };
        }

        private static bool IsUserAbleToOrderProduct(string userRole, string modelNumber)
        {
            return userRole.Equals("viewOnly",StringComparison.CurrentCultureIgnoreCase)
                ? false
                : true;
        }

        private static decimal? GetUserPrice(decimal? listPrice, string userRole)
        {
            return userRole.Equals("VIP", StringComparison.CurrentCultureIgnoreCase)
                ? listPrice == null ? null : listPrice * .5m
                : listPrice == null ? null : listPrice * .8m;
        }

        private static decimal GetListPrice(string modelNumber)
        {
            if(modelNumber.StartsWith("ULTRA", StringComparison.CurrentCultureIgnoreCase))
            {
                return 300m;
            }

            return 50m;
        }
    }
}
