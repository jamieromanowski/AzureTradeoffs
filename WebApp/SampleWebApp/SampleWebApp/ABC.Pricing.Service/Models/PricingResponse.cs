using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCSHttpFunction.ABC.Pricing.Models
{
    public class PricingResponse
    {
        public string ModelNumber { get; set; }
        public decimal? ListPrice { get; set; } 
        public decimal? UserPrice { get; set; }
        public string ErrorReason { get; set; }
    }
}
