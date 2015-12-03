using System.Web;
using System.Web.Mvc;

namespace Sklep_z_truciznami
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
