using Newtonsoft.Json;
using System.Net.Sockets;

namespace matchmaking
{
    public class Player
    {
        public string Token { get; set; }
        internal TcpClient Client { get; set; }
    }
}