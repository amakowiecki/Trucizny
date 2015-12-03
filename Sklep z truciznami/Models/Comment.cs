using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep_z_truciznami.Models
{
    public class Comment
    {
        [Required] [Key]
        public int CommentId { get; set; }

        [Required] 
        public string UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        [Required]
        public string CommentContent { get; set; }

        /// <summary>
        /// czy komentarz jest widoczny (true) czy zotał usunięty (false)
        /// </summary>
        [Required]
        public bool IsVisible { get; set; }
    }
}
