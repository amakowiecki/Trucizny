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
        public ActionResult Index()
        {
            LatestAndMostPopularProducts LatestAndMostPopularProducts = new LatestAndMostPopularProducts();
            LatestAndMostPopularProducts.SetTop10LatestAndMostPopular();

            return View(LatestAndMostPopularProducts);
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
    }
}