using System.Net.Sockets;

namespace FootballClient
{
    public class Player
    {
        public string Token { get; set; }
        internal TcpClient Client { get; set; }

        public Player(string token, TcpClient client)
        {
            Token = token;
            Client = client;
        }
    }
}