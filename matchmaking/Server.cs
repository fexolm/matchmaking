using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace matchmaking
{
    public class Server
    {
        private readonly TcpListener _listener;
        private bool _isRinning = false;

        public Server(int port) {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        private async Task Listen() {
            while (_isRinning) {
                var client = await _listener.AcceptTcpClientAsync();
                Task.Run(() => ProcessConnection(client));
            }
        }

        private static void ProcessConnection(TcpClient client) {
            var request = string.Empty;
            var buffer = new byte[1024];
            for (int count; (count = client.GetStream().Read(buffer, 0, buffer.Length)) > 0;) {
                request += Encoding.UTF8.GetString(buffer, 0, count);
            }
            var response =
                MessageArrived?.Invoke(
                    request, (IPEndPoint) client.Client.RemoteEndPoint) ?? string.Empty;
            buffer = Encoding.UTF8.GetBytes(response);
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();
        }

        public void BeginListen() {
            _isRinning = true;
            Task.Run(async () => await Listen());
        }

        public void StopListen() {
            _isRinning = false;
        }

        public static event Func<string, IPEndPoint, string> MessageArrived;

        public void Send(byte[] msg, IPEndPoint endPoint) {
            _listener.Server.SendTo(msg, endPoint);
        }
    }
}