using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Splitwise.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;

namespace Splitwise.Authentication
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SplitwiseAuthProvider : OAuthAuthorizationServerProvider
    {
        private IUserService _userService;

        public SplitwiseAuthProvider(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            if (_userService.AuthenticateUser(context.UserName, context.Password))            
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.UserName));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "userdisplayname", context.UserName
                    },
                    {
                        "role", "admin"
                    }
                    });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                context.Rejected();
            }
            
            return;
        }
    }
}