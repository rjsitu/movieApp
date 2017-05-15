using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Movies.Startup))]
namespace Movies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
