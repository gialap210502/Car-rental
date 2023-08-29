using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MySignalRApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string connectionId = Context.ConnectionId; // Get the connection ID of the current client
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
