using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sklep_z_truciznami.Startup))]
namespace Sklep_z_truciznami
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
