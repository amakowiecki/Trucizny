using Sklep_z_truciznami.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sklep_z_truciznami.ViewModels
{
    public class ProductVM
    {
        [Required]
        [Key]
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
        [Range(1, 1000, ErrorMessage = "Dostępna ilość może mieć wartość między 1 a 1000")]
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
        public int Rating { get; set; } //ocena

        public static ProductVM ConvertToVM(Product source)
        {
            ProductVM target = new ProductVM();
            target.ProductId = source.ProductId;
            target.SupplierId = source.SupplierId;
            target.ProductName = source.ProductName;
            target.ProductDescription = source.ProductDescription;
            target.Quantity = source.Quantity;
            target.AddDate = source.AddDate;
            target.Category = source.Category;
            target.Price = source.Price;
            target.Unit = source.Unit;
            target.Tags = source.Tags;
            target.PhotoImageFileName = source.PhotoImageFileName;
            target.PhotoImageMimeType = source.PhotoImageMimeType;
            target.Rating = source.RatingNumber != 0 ? source.RatingSum / source.RatingNumber : 0;
            return target;

        }
    }
}