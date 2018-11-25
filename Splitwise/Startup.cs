using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Splitwise.Authentication;
using Splitwise.Service;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Splitwise.Startup))]
namespace Splitwise
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var container = UnityConfig.Container;
            var userService = (IUserService) container.Resolve(typeof(IUserService), null);

            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                Provider = new SplitwiseAuthProvider(userService)
            };

            app.UseOAuthBearerTokens(OAuthOptions);
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}