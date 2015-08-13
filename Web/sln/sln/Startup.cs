using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Michal.Project.Startup))]
namespace Michal.Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
