using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(BMS.App_Start.Startup))]

namespace BMS.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login")
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "917759150215-if1s77dp6tk32om5fhrct7686eb4jbmf.apps.googleusercontent.com",
                ClientSecret = "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"
            });
        }
    }
}
