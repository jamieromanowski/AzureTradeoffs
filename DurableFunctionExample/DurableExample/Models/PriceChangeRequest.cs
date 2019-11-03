using System;

namespace DurableExample.Models
{
    class PriceChangeRequest
    {
        public string ModelNumber { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
