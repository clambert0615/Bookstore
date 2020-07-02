using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public partial class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? PublishDate { get; set; }
        public int? Pages { get; set; }
        public string Genre { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
