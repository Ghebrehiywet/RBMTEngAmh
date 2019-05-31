using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RBMTEngAmh.Startup))]
namespace RBMTEngAmh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
