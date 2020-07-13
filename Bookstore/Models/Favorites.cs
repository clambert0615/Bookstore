using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public partial class Favorites
    {
        public int FavId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int? Pages { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
