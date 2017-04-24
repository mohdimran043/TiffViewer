using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomTiffDemo.Startup))]
namespace CustomTiffDemo
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
