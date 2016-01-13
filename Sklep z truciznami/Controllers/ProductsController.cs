using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sklep_z_truciznami.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Sklep_z_truciznami.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private Product2Context ProductDb = new Product2Context();
        private CommentContext CommentDb = new CommentContext();
        private Rating2Context RatingDb = new Rating2Context();
        string orderLabel = "order";

        public ActionResult Index()
        {
            return View(ProductDb.Products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductDb.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Details Details = new Details(product, CommentDb, RatingDb, User.Identity.Name);

            return View(Details);
        }

        [AllowAnonymous]
        public ActionResult Show(int id)
        {
            byte[] ImageData = ProductDb.FindImageById(id);

            return File(ImageData, "image/jpg");
        }

        public ActionResult AddComment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int id, [Bind(Include = "CommentId,UserId,ProductId,CommentDate,CommentContent,IsVisible")] Comment comment)
        {
            if (comment != null && comment.CommentContent != null && comment.CommentContent != "")
            {
                Comment Comment = new Models.Comment(User.Identity.Name, id, comment.CommentContent);

                CommentDb.Comments.Add(Comment);
                CommentDb.SaveChanges();
            }

            return RedirectToAction("Details", "Products", new { id = id });
        }

        public ActionResult AddRating()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRating(int id, [Bind(Include = "RateId,ClientId,ProductId,Rate")] Rating rating)
        {
            Rating FindRating = RatingDb.FindRating(User.Identity.Name, id);

            if (FindRating != null)
            {
                ProductDb.ChangeProductRating(id, rating.Rate - FindRating.Rate);
                FindRating.Rate = rating.Rate;

                RatingDb.Entry(FindRating).State = EntityState.Modified;
                RatingDb.SaveChanges();
            }
            else
            {
                Rating Rating = new Rating(User.Identity.Name, id, rating.Rate);

                ProductDb.UpdateProductRating(id, Rating.Rate);

                RatingDb.Ratings.Add(Rating);
                RatingDb.SaveChanges();
            }

            return RedirectToAction("Details", "Products", new { id = id });
        }

        [Authorize(Roles = "Owner")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public ActionResult Create([Bind(Include = "ProductId,SupplierId,ProductName,ProductDescription,Quantity,AddDate,Category,Price,Tags,Photo,RatingSum,RatingNumber")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                product.PhotoImageFileName = image.FileName;
                product.PhotoImageMimeType = image.ContentType;
                product.PhotoFile = new byte[image.ContentLength];
                image.InputStream.Read(product.PhotoFile, 0, image.ContentLength);

                product.AddDate = DateTime.Now;

                ProductDb.Products.Add(product);
                ProductDb.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        [Authorize(Roles = "Owner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductDb.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public ActionResult Edit([Bind(Include = "ProductId,SupplierId,ProductName,ProductDescription,Quantity,AddDate,Category,Price,Tags,Photo,RatingSum,RatingNumber")] Product product)
        {
            if (ModelState.IsValid)
            {
                ProductDb.Entry(product).State = EntityState.Modified;
                ProductDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Owner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductDb.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = ProductDb.Products.Find(id);
            ProductDb.Products.Remove(product);
            ProductDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ProductDb.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ListOfProducts()
        {
            return View(ProductDb.Products.ToList());
        }

        public ActionResult Search(string searchPhrase, bool searchInDescription)
        {
            List<Product> productsList;
            if (searchPhrase != "")
            {
                if (searchInDescription) {
                productsList = ProductDb.Products
                    .Where(x => (
                       x.ProductName.ToLower().Contains(searchPhrase.ToLower())
                    || x.Tags.ToLower().Contains(searchPhrase.ToLower())
                    || x.ProductDescription.ToLower().Contains(searchPhrase.ToLower())
                    ))
                    .ToList();
            }
                else
                {                    
                productsList = ProductDb.Products
                    .Where(x => (
                       x.ProductName.ToLower().Contains(searchPhrase.ToLower())
                    || x.Tags.ToLower().Contains(searchPhrase.ToLower())
                    ))
                    .ToList();
                }
            }
            else
            {
                productsList = ProductDb.Products.ToList();
            }

            Sort(productsList);

            return PartialView("PartialListOfProducts", productsList);
        }

        public void Sort(List<Product> productsList) 
        {

        }
        #region ZdublowaneFunkcjeDoKoszyka
        public ActionResult AddProductToCart(int productID, int quantity)
        {
            Session.Timeout = 30;

            Dictionary<Product, int> order;

            if (Session[orderLabel] != null)
            {
                order = Session[orderLabel] as Dictionary<Product, int>;
            }
            else
            {
                order = new Dictionary<Product, int>();
            }

            Product product = ProductDb.Products.Find(productID);

            bool found = false;
            if (order.Count > 0)
            {
                foreach (var item in order)
                {
                    if (item.Key.ProductId == productID)
                    {
                        order[item.Key] = Math.Min(item.Value + quantity, product.Quantity);
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
            {
                if (quantity > 0) { order.Add(product, Math.Min(product.Quantity, quantity)); }
            }

            Session[orderLabel] = order;

            CheckCartForInvalidQuantity();

            return PartialView("PartialCart");
        }

        public ActionResult RemoveProductFromCart(int productID)
        {
            if (Session[orderLabel] == null) { return View("Error"); }

            Dictionary<Product, int> order = Session[orderLabel] as Dictionary<Product, int>;

            Product productToRemove = order.FirstOrDefault(x => x.Key.ProductId == productID).Key;

            if (productToRemove != null)
            {
                order.Remove(productToRemove);
            }

            Session[orderLabel] = order;

            CheckCartForInvalidQuantity();

            return View("PartialCart");
        }

        public ActionResult EditProductInCart(int productID, int newQuantity)
        {
            RemoveProductFromCart(productID);
            AddProductToCart(productID, newQuantity);
            CheckCartForInvalidQuantity();
            return View("PartialCart");
        }

        // CheckCartForInvalidQuantity
        public void CheckCartForInvalidQuantity()
        {
            Dictionary<Product, int> order;

            if (Session[orderLabel] != null)
            {
                order = Session[orderLabel] as Dictionary<Product, int>;
                foreach (var orderPair in order)
                {
                    if (orderPair.Value <= 0)
                    {
                        order.Remove(orderPair.Key);
                    }
                }
                Session[orderLabel] = order;
            }
        }
        #endregion
    }
}
