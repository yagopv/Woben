using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

using Woben.Domain.Model;
using Woben.Web.Helpers;
using Woben.Web.Resources;

namespace Woben.Web.Providers
{
    /// <summary>
    /// oAuth authorization server
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly Func<ApplicationUserManager> _userManagerFactory;

        /// <summary>
        /// ctor
        /// </summary>		
        public ApplicationOAuthProvider(string publicClientId, Func<ApplicationUserManager> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            _publicClientId = publicClientId;
            _userManagerFactory = userManagerFactory;
        }	

        /// <summary>
        /// oAuth Resource Password Login Flow
		/// 1. Checks the password with the Identity API
		/// 2. Create a user identity for the bearer token
		/// 3. Create a user identity for the cookie
		/// 4. Calls the context.Validated(ticket) to tell the oAuth2 server to protect the ticket as an access token and send it out in JSON payload
		/// 5. Signs the cookie identity so it can send the authentication cookie
        /// </summary>
        /// <param name="context">The authorization context</param>
		/// <returns>Task</returns>		
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (ApplicationUserManager userManager = _userManagerFactory())
            {
                userManager.MaxFailedAccessAttemptsBeforeLockout = 5;
                userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);

                UserProfile user = await userManager.FindByNameAsync(context.UserName);

                if (user == null)
                {
                    context.SetError("invalid_grant", WebResources.InvalidUser);
                    return;
                }

                if (await userManager.IsLockedOutAsync(user.Id))
                {
                    var timeleft = user.LockoutEndDateUtc.GetValueOrDefault().Subtract(DateTime.UtcNow);

                    var timetype = timeleft.Minutes == 0 ? WebResources.Seconds : WebResources.Minutes;
                    var timevalue = timeleft.Minutes == 0 ? timeleft.Seconds : timeleft.Minutes;

                    context.SetError("invalid_grant", string.Format(WebResources.AccountLocked, timevalue, timetype));
                    return;
                }

                if (!(await userManager.CheckPasswordAsync(user, context.Password)))
                {
                    await userManager.AccessFailedAsync(user.Id);

                    if (await userManager.IsLockedOutAsync(user.Id))
                    {
                        context.SetError("invalid_grant", string.Format(WebResources.AccountLockedForMinutes, userManager.DefaultAccountLockoutTimeSpan.Minutes));
                        return;
                    }

                    var possibleAttempts = userManager.MaxFailedAccessAttemptsBeforeLockout;
                    var currentcount = await userManager.GetAccessFailedCountAsync(user.Id);
                    
                    context.SetError("invalid_grant", string.Format(WebResources.InvalidPassword, possibleAttempts - currentcount));
                    return;
                }

                await userManager.ResetAccessFailedCountAsync(user.Id);
                
                ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                    context.Options.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);

                var justCreatedIdentity = await userManager.FindByNameAsync(user.UserName);
                var roles = await userManager.GetRolesAsync(justCreatedIdentity.Id);

                AuthenticationProperties properties = CreateProperties(user, roles.ToArray());
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        /// <summary>
        /// Add parameters to the response
        /// </summary>
        /// <param name="context">Endpoint context</param>
        /// <returns>Task</returns>		
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Password resource owner credentials don´t provide a client identifier
        /// </summary>
        /// <param name="context">Validate context</param>
        /// <returns>Task</returns>		
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Valildate the redirect uri
        /// </summary>
        /// <param name="context">Validate context</param>
        /// <returns>Task</returns>		
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Create the authentication properties
		/// Create the required properties that would be converted into Claims
        /// </summary>
        /// <param name="userName">The user name</param>
		/// <param name="roles">The user roles</param>
        /// <returns>The properties</returns>
        public static AuthenticationProperties CreateProperties(UserProfile user,  string[] roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName },
                { "roles", String.Join("," , roles) },
                { "isEmailConfirmed", user.EmailConfirmed.ToString().ToLower() },
                { "name", user.Name },
                { "firstName", user.FirstName },
                { "lastName", user.Lastname },
                { "phoneNumber", user.PhoneNumber != null ? user.PhoneNumber : ""  },
                { "email", user.Email  }

            };
            return new AuthenticationProperties(data);
        }
    }
}