using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace IBHFL
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            IAppBuilder app1 = app;
            CookieAuthenticationOptions options = new CookieAuthenticationOptions();
            options.AuthenticationType = "ApplicationCookie";
            options.LoginPath = new PathString("/Account/Login");
            app1.UseCookieAuthentication(options);
            app.UseExternalSignInCookie("ExternalCookie");
        }

        public void Configuration(IAppBuilder app) => this.ConfigureAuth(app);
    }
}
