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
    public static class EnumsHelper
    {
        public static string GetName(Models.Category category)
        {
            DisplayAttribute[] attributes = (DisplayAttribute[])category.GetType().GetField(category.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : string.Empty;
        }

        public static string GetName(Models.OrderStatus orderStatus)
        {
            DisplayAttribute[] attributes = (DisplayAttribute[])orderStatus.GetType().GetField(orderStatus.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : string.Empty;
        }

        public static string GetName(Models.Unit unit)
        {
            DisplayAttribute[] attributes = (DisplayAttribute[])unit.GetType().GetField(unit.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : string.Empty;
        }


    }

    public static class DbHelper
    {
        public static void RecreateDatabase()
        { 
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.Order2Context, Migrations.Configuration>());
            //Database.SetInitializer(new Drop<Models.Order2Context>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Models.Product2Context>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Models.Order2Context>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Models.Rating2Context>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Models.CommentContext>());
        }
    }

}