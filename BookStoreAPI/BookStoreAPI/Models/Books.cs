using System;
using System.Collections.Generic;

namespace BookStoreAPI.Models
{
    public partial class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? Pages { get; set; }
        public string Genre { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
    }
}
