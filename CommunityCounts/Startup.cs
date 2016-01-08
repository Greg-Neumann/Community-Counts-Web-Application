using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommunityCounts.Startup))]
namespace CommunityCounts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
