using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sklep_z_truciznami.Models
{
    public class AdressDataModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAndNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}