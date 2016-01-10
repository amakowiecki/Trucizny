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
            if (comment!= null && comment.CommentContent!= null &&comment.CommentContent != "")
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
    }
}
