using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sln.Startup))]
namespace sln
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
