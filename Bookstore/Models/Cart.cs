using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Cart
    {
        public Books Book { get; set; }
        public int Quantity { get; set; }
        //public virtual AspNetUsers User { get; set; }


        public Cart(Books book, int quantity)
        {
            Book = book;
            Quantity = quantity;

        }

        public Cart()
        {
        }
    }
}
