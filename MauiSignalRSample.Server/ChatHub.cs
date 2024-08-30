using Microsoft.AspNetCore.SignalR;

namespace MauiSignalRSample.Server
{
    public class ChatHub : Hub
    {
        private static int connectedUsers = 0;

        public override async Task OnConnectedAsync()
        {
            if (connectedUsers >= 2)
            {
                await Clients.Caller.SendAsync("MessageReceived", "Server is full. Only two clients are allowed.");
                Context.Abort();
                return;
            }

            connectedUsers++;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            connectedUsers = Math.Max(0, connectedUsers - 1);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("MessageReceived", message);
        }
    }
}
