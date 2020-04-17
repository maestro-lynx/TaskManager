using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("OwinConfiguration",typeof(TaskManager.WEB.Startup))]
namespace TaskManager.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        
    }
}