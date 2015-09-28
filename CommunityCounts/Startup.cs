using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using CommunityCounts.Models;

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
