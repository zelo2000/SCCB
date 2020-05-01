using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SCCB.Web.Hubs
{
    public class BookingHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
