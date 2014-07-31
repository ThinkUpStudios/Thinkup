using Facebook;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;
using System;
using System.Configuration;
using System.Security.Claims;

namespace ThinkUp.SignalR
{
    public class ThinkUpAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            var isAuthorized = false;
            var userName = string.Empty;
            var authenticationType = default(AuthenticationType);
            var authenticationTypeValue = request.QueryString.Get("authenticationType");

            if (Enum.TryParse(authenticationTypeValue, out authenticationType))
            {
                switch (authenticationType)
                {
                    case AuthenticationType.Facebook:
                        var authenticationToken = request.QueryString.Get("authenticationToken");
                        var appId = ConfigurationManager.AppSettings["FacebookAppId"];
                        var appSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
                        var facebookClient = new FacebookClient
                        {
                            AppId = appId,
                            AppSecret = appSecret,
                            AccessToken = authenticationToken
                        };
                        dynamic connectedUser = facebookClient.Get("me");

                        if (connectedUser != null)
                        {
                            isAuthorized = true;
                            userName = connectedUser.username;
                        }
                        break;
                    case AuthenticationType.None:
                        isAuthorized = true;
                        userName = request.QueryString.Get("userName");

                        if (string.IsNullOrEmpty(userName))
                        {
                            userName = string.Format("User-{0}", Guid.NewGuid());
                        }
                        break;
                }

                var identity = new ClaimsIdentity("ThinkUpAuthorization");

                identity.AddClaim(new Claim(ClaimTypes.Name, userName));

                var principal = new ClaimsPrincipal(identity);

                request.Environment["authorizedUser"] = principal;
            }

            return isAuthorized;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
            var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
            var principal = environment["authorizedUser"] as ClaimsPrincipal;

            if (principal != null && principal.Identity != null && principal.Identity.IsAuthenticated)
            {
                hubIncomingInvokerContext.Hub.Context = new HubCallerContext(new ServerRequest(environment), connectionId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}