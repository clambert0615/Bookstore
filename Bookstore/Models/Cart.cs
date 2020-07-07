using System;
using System.Collections.Generic;

namespace Bookstore.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int? BookId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public Books Book { get; set; }
        
           
        public virtual AspNetUsers User { get; set; }

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
