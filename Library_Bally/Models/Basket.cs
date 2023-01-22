using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Bally.Models
{
    public class Basket
    {
        public int BookID { get; set; }

        public string BookName { get; set; }

        public decimal Price { get; set; }

        public double DiscountedPrice { get; set; }
    }
}