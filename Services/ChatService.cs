using System.ComponentModel;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketChat.Services
{
    public class ChatService
    {
        /// <summary>
        /// WebSocket - ClientName dictionary
        /// </summary>
        private readonly Dictionary<WebSocket, string> Connections;

        public ChatService()
        {
            Connections = new();
        }

        ~ChatService()
        {
            Parallel.ForEach(Connections, async (item) =>
                await item.Key.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Server Shutdown", CancellationToken.None));
        }

        public async Task NewClientTask(WebSocket ws, string nick)
        {
            Connections.Add(ws, nick);
            SendMessageToAllClients($"Server: User {nick} joined");
            while (true)
            {
                byte[] buffer = new byte[4096];
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                if (!ws.State.Equals(WebSocketState.Open))
                    break;
                var line = Encoding.UTF8.GetString(buffer[..(result.Count)]);
                if (line.StartsWith("/"))
                    await TryParsingCommand(ws, line[1..]);
                else
                    SendMessageToAllClients($"{Connections[ws]}: {line}");
            }
            Connections.Remove(ws);
            ws.Dispose();
            SendMessageToAllClients($"Server: User {nick} left");
        }

        private void SendMessageToAllClients(string msg)
        {
            var encodedMessage = Encoding.UTF8.GetBytes(msg);
            Parallel.ForEach(Connections, async (client) => await client.Key.SendAsync(encodedMessage, WebSocketMessageType.Text, true, CancellationToken.None));
        }

        private static async Task SendPrivateMessage(string msg, WebSocket ws)
        {
            var encodedMessage = Encoding.UTF8.GetBytes(msg);
            await ws.SendAsync(encodedMessage, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task TryParsingCommand(WebSocket caller, string inputLine)
        {
            var commandName = inputLine.Split(' ');
            var success = commandName[0] switch
            {
                "w" => await TrySendPrivateMessage(caller, commandName[1..]),
                _ => false
            };
            if (!success)
            {
                await SendPrivateMessage($"Server: Invalid command", caller);
            }
        }

        private async Task<bool> TrySendPrivateMessage(WebSocket caller, params string[] inputLine)
        {
            if (inputLine.Length < 2)
                return false;

            var to = inputLine[0];// 
            var message = string.Join(" ", inputLine[1..]);
            if (to == null || message == null)
                return false;

            try
            {
                var targetConnection = Connections.First((x) => x.Value == to).Key;
                await SendPrivateMessage($"Private: {Connections[caller]} whispered: \"{message}\"", targetConnection);
            }
            catch
            {
                await SendPrivateMessage($"Server: Invalid user specificated", caller);
            }

            return true;
        }
    }

}