using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sklep_z_truciznami
{
    public static class CategoryHelper
    {
        public static string GetName(Models.Category category)
        {
            DisplayAttribute[] attributes = (DisplayAttribute[])category.GetType().GetField(category.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : string.Empty;
        }
        
    }

}