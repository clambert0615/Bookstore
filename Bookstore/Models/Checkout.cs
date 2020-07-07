using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Checkout
    {
        public Books Book { get; set; }
        public Cart Cart { get; set; }
     
   
        public decimal? Subtotal { get; set; }
        public decimal? SalesTax { get; set; }
        public decimal? Total { get; set; }

        public List<CheckoutItem> CheckoutItem { get; set; }
        public List<Checkout> CheckoutList  {get; set;}
    }
}
