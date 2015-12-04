using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep_z_truciznami.Models
{
    public class Rating
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Rate { get; set; }

    }
}
