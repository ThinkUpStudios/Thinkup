using Microsoft.AspNet.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ThinkUp.SignalR
{
    [ThinkUpAuthorize]
    public class ThinkUpHub: Hub<IClientHub>
    {
        private readonly IClientManager clientManager;

        public ThinkUpHub(IClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        public override Task OnConnected()
        {
            var userName = this.GetUserName();

            this.clientManager.Connect(userName, Context.ConnectionId);

            return base.OnConnected();
        }

        public void SendMessage(string message)
        {
            this.clientManager.SendMessage(message, Context.ConnectionId);
        }

        public override Task OnReconnected()
        {
            var userName = this.GetUserName();

            this.clientManager.Reconnect(userName, Context.ConnectionId);

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userName = this.GetUserName();

            this.clientManager.Disconnect(userName, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        private string GetUserName()
        {
            var userName = Context.User.Identity.Name;

            if (string.IsNullOrEmpty(userName) && Context.Request.Environment.ContainsKey("authorizedUser"))
            {
                var principal = Context.Request.Environment["authorizedUser"] as ClaimsPrincipal;

                userName = principal.Identity.Name;
            }

            return userName;
        }
    }
}