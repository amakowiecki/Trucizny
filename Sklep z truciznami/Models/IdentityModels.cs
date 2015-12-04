using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Sklep_z_truciznami.Models
{
    public enum AccountType
    {
        User,
        Owner
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nazwa użytkownika")]
        public string Login { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; } 

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } //imię i nazwisko do celów wysyłki zamówienia

        [Display(Name = "Ulica i numer domu")]
        public string StreetAndNumber { get; set; }

        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }


        public bool IsActive { get; set; } //tego oczywiście nie chcemy wyświetlać użytkownikowi
        public AccountType UserType { get; set; }   //tego też


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}