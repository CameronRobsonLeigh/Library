using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Bally.Models;

namespace Library_Bally.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
          
        }

        // method that creates our list of our non-discounted books from our base class.
        public List<Book> GetBooks()
        {
            List<Book> Book = new List<Book>()
            {
                new Book() { BookID = 1, Author = "Herman Melville", BookName = "Moby Dick", Genre = "Adventure Fiction", Price = 15.20m, Year = 1851},
                new Book() { BookID = 2, Author = "Tom Robbins", BookName = "Still Life With Woodpecker", Genre = "Romance Novel", Price = 11.05m, Year = 1980},
                new Book() { BookID = 3, Author = "Agatha Christie", BookName = "Sleeping Murder", Genre = "Detective Novel", Price = 10.24m, Year = 1976},
                new Book() { BookID = 4, Author = "Jerome K. Jerome", BookName = "Three Men in a Boat", Genre = "Novel ", Price = 12.87m, Year = 1889},
                new Book() { BookID = 5, Author = "H. G. Wells", BookName = "The Time Machine", Genre = "Time Travel fiction", Price = 10.43m, Year = 1895},
                new Book() { BookID = 6, Author = "Isaac Asimov", BookName = "The Caves of Steel", Genre = "Science fiction", Price = 8.12m, Year = 1954},
                new Book() { BookID = 7, Author = "Jerome K. Jerome", BookName = "Idle Thoughts of an Idle Fellow", Genre = "Fiction", Price = 7.32m, Year = 1886},
                new Book() { BookID = 8, Author = "Charles Dickens", BookName = "A Christmas Carol", Genre = "Historical Fiction", Price = 4.23m, Year = 1843},
                new Book() { BookID = 9, Author = "Charles Dickens", BookName = "A Tale of Two Cities", Genre = "Historical Novel", Price = 6.32m, Year = 1859},
                new Book() { BookID = 10, Author = "Charles Dickens", BookName = "Great Expectations", Genre = "Novel", Price = 13.21m, Year = 1861}
            };
            return Book;
        }

        // method that creates our list of ten percent discounted books, inherits from base class adding a discount rate of 0.1
        // here you can add any discount for any book, for example, if we wanted a 30% discount for any book after 2015 then you would just add it to the list
        public List<DiscountedBook> DiscountedBooks()
        {
            List<DiscountedBook> discountedList = new List<DiscountedBook>()
            {
                new DiscountedBook() { BookID = 1000000, Author = "Jonathon Coe", BookName = "The Terrible Privacy of Maxwell Sim", Genre = "Picaresque novel", Price = 13.14m, Year = 2010, DiscountRate = 10}

            };
            return discountedList;
        }

        // a method that creates our total basket value list's, here we can easily add total value discounts
        public List<BasketDiscounts> BasketDiscounts()
        {
            List<BasketDiscounts> basketDiscountList = new List<BasketDiscounts>()
            {
                new BasketDiscounts() { BasketValue = 30.0m, Discount = 5 }
            };
            return basketDiscountList;
        }


        // Expando object allows us to pass multiple models to the view
        public ActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.NonDiscountedBooks = GetBooks();
            mymodel.DiscountedBooks = DiscountedBooks();
            return View(mymodel);
        }

        // ajax post add to basket 
        [HttpPost]
        public ActionResult AddToBasket(int BookID)
        {
            // find book type from what was passed through from the user
            List<DiscountedBook> getDiscountBook = DiscountedBooks().Where(m => m.BookID == BookID).ToList();
            List<Book> getBook = GetBooks().Where(m => m.BookID == BookID).ToList();

            // create basket list
            List<Basket> UsersBasket = new List<Basket>();

            // detect what type of book comes through - discounted or normal price.
            if (getBook.Count ==  1)
            {
                UsersBasket.Add(new Basket() { BookID = BookID, BookName = getBook[0].BookName, Price = getBook[0].Price });
            }
            else
            {
                UsersBasket.Add(new Basket() { BookID = BookID, BookName = getDiscountBook[0].BookName, Price = getDiscountBook[0].Price, DiscountedPrice = getDiscountBook[0].DiscountRate });
            }
            
            return Json(new { success = true, message = UsersBasket[0].BookID + "#]" + UsersBasket[0].BookName + "#]" + UsersBasket[0].Price + "#]" + UsersBasket[0].DiscountedPrice });
        }

        [HttpPost]
        public ActionResult CheckForTotalDiscount(decimal BasketPrice)
        {
            // create list to check our basket price
            List<decimal> discountValues = new List<decimal>();
            
            // loop through our range of discounts and push into list
            foreach (BasketDiscounts values in BasketDiscounts())
            {
                discountValues.Add(values.BasketValue);
            };

            // sort values asc
            discountValues.Sort();

            // find the closest discount to what has been passed through
            decimal valuePassed = BasketPrice;
            decimal closest = discountValues.Aggregate((x, y) => Math.Abs(x - valuePassed) < Math.Abs(y - valuePassed) ? x : y);

            decimal nextLowest;
            List<BasketDiscounts> getOffer = new List<BasketDiscounts>();
            // get the last previous value using linq, here we are saying if we go over one of the values, then get that discount that we have just surpassed.
            nextLowest = discountValues.LastOrDefault(x => x < BasketPrice);
            // get the offer of the valid discount
            getOffer = BasketDiscounts().Where(m => m.BasketValue == nextLowest).ToList();
            
            // if we don't hit the criteria for discount then return nothing
            if (getOffer.Count == 0)
            {
                return Json(new { success = true, message = "" });

            } 
            else
            {
                return Json(new { success = true, message = getOffer[0].Discount });
            }
        }      
    }
}