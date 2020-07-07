using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class CheckoutItem
    {
        public Books Book { get; set; }
        public int Quantity { get; set; }
        public decimal? Subtotal { get; set; }

    }
}
