using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Payment
    {
        public decimal Total { get; set; }
        public string CCType { get; set; }
        public string Name { get; set; }
        public string CCNum { get; set; }
        public string CCV { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string BillName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public bool SameAdr { get; set; }
    }
}
