using System.Net.Sockets;

namespace FootballClient
{
    public class Player
    {
        public Token Token { get; set; }
        internal TcpClient Client { get; set; }

        public Player(Token token, TcpClient client)
        {
            Token = token;
            Client = client;
        }
    }
}