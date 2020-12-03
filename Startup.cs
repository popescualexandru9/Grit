using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Grit.Startup))]
namespace Grit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
