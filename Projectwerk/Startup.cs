using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projectwerk.Startup))]
namespace Projectwerk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
