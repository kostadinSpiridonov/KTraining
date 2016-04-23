using Microsoft.Owin;
using Owin;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(KTraining.Startup))]
namespace KTraining
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
