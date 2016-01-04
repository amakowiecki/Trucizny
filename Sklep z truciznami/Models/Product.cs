using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep_z_truciznami.Models
{
    public enum Category
    {
        [Display(Name = "Jadalne")]
        Edible,

        [Display(Name = "Skórne")]
        Dermal,

        [Display(Name ="Wziewne")]
        Inhaled,

        [Display(Name = "Dożylne")]
        Intravenous
    }
    public class Product
    {
        [Required][Key]
        public int ProductId { get; set; }
        public string SupplierId { get; set; } //gdyby potencjalnie było kilku dostawców

        [Required]
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Opis produktu")]
        public string ProductDescription { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Dostępna ilość")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Data dodania")]
        public DateTime AddDate { get; set; }

        [Required]
        [Display(Name = "Kategoria")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cena za sztukę")]
        public double Price { get; set; }


        /// <summary>
        /// tagi produktu oddzielane przecinkami
        /// </summary>
        [Required]
        [Display(Name = "Tagi")]
        public string Tags { get; set; }


        //public byte[] Photo { get; set; } //typ System.Drawing.Image, nie jestem pewien czy taki będzie dobry
        //public Photo Photo { get; set; }


        public string PhotoImageFileName { get; set; }
        public string PhotoImageMimeType { get; set; }
        public byte[] PhotoFile { get; set; }


        //oceny wyliczane dla produktu jako iloraz RatingSum/RatingNumber
        public int RatingSum { get; set; } //suma ocen
        public int RatingNumber { get; set; } //ilość ocen

        public void IncrementRatingSum()
        {
            RatingSum++;
        }
    }

    public class Product2Context : DbContext
    {
        public Product2Context()
            : base("DefaultConnection")
        {
        }

        public DbSet<Product> Products { get; set; }

        public byte[] FindImageById(int id)
        {
            byte[] ImageData = (from x in Products
                             where x.ProductId == id
                             select x.PhotoFile).SingleOrDefault();

            return ImageData;
        }

        public void UpdateProductRating(int productId, int rate)
        {
            Product product = Products.Find(productId);

            product.RatingSum += rate;
            product.RatingNumber++;

            this.Entry(product).State = EntityState.Modified;
            this.SaveChanges();
        }

        public void ChangeProductRating(int productId, int rate)
        {
            Product product = Products.Find(productId);

            product.RatingSum += rate;

            this.Entry(product).State = EntityState.Modified;
            this.SaveChanges();
        }
    }
}
