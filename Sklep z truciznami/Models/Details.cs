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
        public IList<Comment> Comments { get; set; }

        private CommentContext CommentDb;

        public Details(Product product, CommentContext commentDb)
        {
            Product = product;
            CommentDb = commentDb;

            SetProductComments();
        }

        private void SetProductComments()
        {
            var comments = (from x in CommentDb.Comments
                            where x.ProductId == Product.ProductId
                            select x).ToList();

            Comments = comments;
        }
    }
}