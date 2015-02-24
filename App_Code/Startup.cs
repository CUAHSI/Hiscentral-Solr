using Microsoft.Owin;
using Owin;
// changed from OwinStartupAttribute to startup
[assembly: OwinStartup (typeof(WebSite1.Startup))]
namespace WebSite1
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
