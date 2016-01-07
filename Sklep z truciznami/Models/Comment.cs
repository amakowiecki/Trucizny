using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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

        [Required(ErrorMessage = "Wypełnij treść komentarza")]
        [Display(Name = "Treść Komentarza")]
        public string CommentContent { get; set; }

        /// <summary>
        /// czy komentarz jest widoczny (true) czy zotał usunięty (false)
        /// </summary>
        [Required]
        public bool IsVisible { get; set; }

        public Comment()
        {

        }

        public Comment(string userName, int produtId, string commentContent)
        {
            this.UserId = GetUserId(userName);

            this.ProductId = produtId;
            this.CommentContent = commentContent;
            this.CommentDate = DateTime.Today;
            this.IsVisible = true;
        }

        private string GetUserId(string userName)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;

            return user.Id;
        }
    }

    public class CommentContext : DbContext
    {
        public CommentContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Comment> Comments { get; set; }
    }
}
