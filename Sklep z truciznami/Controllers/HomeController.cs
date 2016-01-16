using Sklep_z_truciznami.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sklep_z_truciznami.Controllers
{
    public class HomeController : Controller
    {
        private Models.Product2Context ProductDb = new Product2Context();

        public ActionResult Index()
        {
            LatestAndMostPopularProducts LatestAndMostPopularProducts = new LatestAndMostPopularProducts();
            LatestAndMostPopularProducts.SetTop10LatestAndMostPopular();
            ViewBag.ListView = false;
            return View(LatestAndMostPopularProducts);
        }

        public ActionResult ListOfProducts(string searchPhrase)
        {
            return RedirectToAction("SearchMain", "Products", new { searchPhrase = searchPhrase});
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Shipping()
        {
            return View();
        }

        public ActionResult ValentinesDay()
        {
            var chloroform = ProductDb.Products.Where(x => x.ProductName.ToLower().Contains("chloroform")).FirstOrDefault();
            if (chloroform != null)
            {
                ViewBag.chloroformId = chloroform.ProductId;
            }
            var lovage = ProductDb.Products.Where(x => x.ProductName.ToLower().Contains("lubczyk")).FirstOrDefault();
            if (lovage != null)
            {
                ViewBag.lovageId = lovage.ProductId;
            }

            return View();
        }

        public ActionResult Sale()
        {
            var fir = ProductDb.Products.Where(x => x.ProductName.ToLower().Contains("jodła")).FirstOrDefault();
            if (fir != null)
            {
                ViewBag.firId = fir.ProductId;
            }
            var mistletoe = ProductDb.Products.Where(x => x.ProductName.ToLower().Contains("jemioła")).FirstOrDefault();
            if (mistletoe != null)
            {
                ViewBag.mistletoeId = mistletoe.ProductId;
            }
            return View();
        }
    }
}