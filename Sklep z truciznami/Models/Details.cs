using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sklep_z_truciznami.Models
{
    public class Details
    {
        public Product Product { get; set; }
        public Comment Comment { get; set; }
        public Rating Rating { get; set; }
        public IList<Comment> Comments { get; set; }

        private CommentContext CommentDb;
        private Rating2Context RatingContext;

        public Details(Product product, CommentContext commentDb, Rating2Context ratingContext, string userName)
        {
            Product = product;
            CommentDb = commentDb;
            RatingContext = ratingContext;

            Rating = RatingContext.FindRating(userName, Product.ProductId);


            SetProductComments();
        }

        private void SetProductComments()
        {
            var comments = (from x in CommentDb.Comments
                            where x.ProductId == Product.ProductId
                            select x).ToList();

            foreach (var comment in comments)
            {
                if (comment.IsVisible == false) comment.CommentContent = "Komentarz został usunięty.";
            }

            Comments = comments;
        }
    }
}