using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep_z_truciznami.Models
{
    public enum OrderStatus
    {
        /// <summary>
        /// Złożone przez klienta
        /// </summary>
        [Display(Name = "Złożone")]
        Ordered,

        /// <summary>
        /// Anulowane przez klienta
        /// </summary>
        [Display(Name = "Anulowane")]
        Cancelled,  

        /// <summary>
        /// Zaakceptowane przez właściciela
        /// </summary>
        [Display(Name = "Zrealizowane")]
        Realized,


        /// <summary>
        /// Odrzucone przez właściciela
        /// </summary>
        [Display(Name = "Odrzucone")]
        Rejected
        
    }
   public class Order
    {
        [Required] [Key]
        public int OrderId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public int ProductId { get; set; } //tego nie chcemy w widoku

        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        /// <summary>
        /// Zamawiana ilość
        /// </summary>
        [Required]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; } 

        [Required]
        [Display(Name = "Data zamówienia")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Data realizacji")]
        public DateTime? AnswerDate { get; set; }

        [Required]
        [Display(Name = "Status zamówienia")]
        public OrderStatus Status { get; set; }


    }

   public class Order2Context : DbContext
   {
       public Order2Context()
           : base("DefaultConnection")
       {
       }

       public DbSet<Order> Orders { get; set; }
   }
}
