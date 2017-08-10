using System.Net;

namespace FootballClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MMClient Ars = new MMClient(Token.Generate());
            Ars.Connect(IPAddress.Parse("127.0.0.1"), 8001);
            
            while (true)
            {

            }
        }
    }
}
