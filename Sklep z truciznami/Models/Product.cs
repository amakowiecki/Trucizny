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
    public enum Unit
    {
        [Display(Name = "Fiolka 10ml")]
        Phial10ml,

        [Display(Name = "Woreczek 20g")]
        Bag20g,

        [Display(Name = "Sztuka")]
        Piece
    }
    public class Product
    {
        [Required][Key]
        public int ProductId { get; set; }
        public string SupplierId { get; set; } //gdyby potencjalnie było kilku dostawców

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(300)]
        [Display(Name = "Opis produktu")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Range(1, 1000, ErrorMessage ="Dostępna ilość może mieć wartość między 1 a 1000")]
        [Display(Name = "Dostępna ilość")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Data dodania")]
        public DateTime AddDate { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Kategoria")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Cena za sztukę")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Jednostka")]
        public Unit Unit { get; set; }

        /// <summary>
        /// tagi produktu oddzielane przecinkami
        /// </summary>
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Tagi")]
        public string Tags { get; set; }


        //public byte[] Photo { get; set; } //typ System.Drawing.Image, nie jestem pewien czy taki będzie dobry
        //public Photo Photo { get; set; }


        public string PhotoImageFileName { get; set; }
        public string PhotoImageMimeType { get; set; }
        [Display(Name = "Zdjęcie")]
        public byte[] PhotoFile { get; set; }


        //oceny wyliczane dla produktu jako iloraz RatingSum/RatingNumber
        public int RatingSum { get; set; } //suma ocen
        public int RatingNumber { get; set; } //ilość ocen

        public void IncrementRatingSum()
        {
            RatingSum++;
        }

        static public string GetCategoryDisplayName(string name)
        {
            string DisplayName = (typeof(Category).GetMember(name)[0].CustomAttributes.First().NamedArguments.First().TypedValue.Value).ToString();

            return DisplayName;
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
