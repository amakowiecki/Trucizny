using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sklep_z_truciznami.Models
{
    public class LatestAndMostPopularProducts
    {
        public List<Product> Latest { get; set; }
        public List<Product> MostPopular { get; set; }

        Product2Context Db;

        public LatestAndMostPopularProducts()
        {
            Db = new Product2Context();
        }

        public LatestAndMostPopularProducts SetTop10LatestAndMostPopular()
        {
            GetLatestProducts();
            GetMostPoplarProducts();

            return this;
        }

        private void GetLatestProducts()
        {
            Latest = (from x in Db.Products
                      orderby x.AddDate descending
                      select x)
                          .Take(10).ToList();
        }

        private void GetMostPoplarProducts()
        {
            MostPopular = (from x in Db.Products
                           where x.RatingSum > 0
                           let rating = x.RatingNumber / x.RatingSum
                           orderby rating descending
                           select x)
                               .Take(10).ToList();
        }
    }
}