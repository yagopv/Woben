using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Woben.Web.Startup))]
namespace Woben.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
