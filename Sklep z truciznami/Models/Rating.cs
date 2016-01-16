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
    public class Rating
    {
        [Key]
        [Required]
        public int RateId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Rate { get; set; }

        public Rating()
        {

        }

        public Rating(string userName, int productId, int rate)
        {
            ClientId = GetUserId(userName);
            ProductId = productId;
            Rate = rate;
        }

        private string GetUserId(string userName)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;

            return user.Id;
        }
    }

    //public class RatingContext : DbContext
    //{
    //    public RatingContext()
    //        : base("DefaultConnection")
    //    {
    //    }

    //    public DbSet<Rating> Ratings { get; set; }

    //    public bool IsRated(string clientId, int productId)
    //    {
    //        var Rate = from x in Ratings
    //                   where x.ClientId == clientId && x.ProductId == productId
    //                   select x;

    //        if (Rate == null)
    //            return true;

    //        return false;
    //    }
    //}

    public class Rating2Context : DbContext
    {
        public Rating2Context()
            : base("DefaultConnection")
        {
        }

        public DbSet<Rating> Ratings { get; set; }

        public Rating FindRating(string userName, int productId)
        {
            string clientId = GetUserId(userName);

            var Rate = (from x in Ratings
                       where x.ClientId == clientId && x.ProductId == productId
                       select x).SingleOrDefault();

            return Rate;
        }

        //public Rating FindRatingWithClientId(string clientId, int productId)
        //{
        //    var Rate = (from x in Ratings
        //                where x.ClientId == clientId && x.ProductId == productId
        //                select x).SingleOrDefault();

        //    return Rate;
        //}

        private string GetUserId(string userName)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;

            if (user == null)
                return "";

            return user.Id;
        }
    }
}
