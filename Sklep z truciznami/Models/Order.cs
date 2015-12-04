using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int ProductId { get; set; }

        /// <summary>
        /// Zamawiana ilość
        /// </summary>
        [Required]
        public int Quantity { get; set; } 

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime AnswerDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }


    }
}
