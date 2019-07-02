using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shopalooza.WebUI.Startup))]
namespace Shopalooza.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
