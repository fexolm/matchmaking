using System.Net.Sockets;

namespace matchmaking
{
    public class Player
    {
        public string Token { get; internal set; }
        internal TcpClient Client { get; set; }
    }
}