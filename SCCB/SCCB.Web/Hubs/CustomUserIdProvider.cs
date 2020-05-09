using Microsoft.AspNetCore.SignalR;
using SCCB.Core.Constants;

namespace SCCB.Web.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            if (connection.User.Identity.IsAuthenticated)
            {
                return connection.User.FindFirst(ClaimKeys.Id).Value;
            }

            return "";
        }
    }
}
