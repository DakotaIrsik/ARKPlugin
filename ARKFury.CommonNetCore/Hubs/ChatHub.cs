
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ArkFury.CommonWeb.Hubs
{

    public class ChatHub : Hub
    {

        [HubMethodName("SendMessage")]
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("broadcastchartdata", user, message);
        }
    }
}