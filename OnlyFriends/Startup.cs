using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlyFriends.Startup))]
namespace OnlyFriends
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

// helklloo