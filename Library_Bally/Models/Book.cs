using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Bally.Models
{
    // Base class
    public class Book
    {
        public int BookID { get; set; }

        public string BookName { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public string Genre { get; set; }

        public string Author { get; set; }

    }

    // inheritence from the base class so we can add new discount rates for particular books.
    public class DiscountedBook : Book
    {
        public double DiscountRate;
    }

}