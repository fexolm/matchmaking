using System.Net;
using System.Timers;

namespace FootballClient
{
    class Program
    {
        static MMClient client;

        static void Main(string[] args)
        {
            var client = new MMClient(Token.Generate());
            client.Connect(IPAddress.Parse("127.0.0.1"), 8001);

            client.OnInviteAccepted = System.Console.WriteLine;
            client.OnInviteRejected = System.Console.WriteLine;
            client.OnInviteRecieved = System.Console.WriteLine;
            client.OnInviteError = System.Console.WriteLine;

            var timer = new Timer();
            timer.Interval = 100;
            timer.Elapsed += new ElapsedEventHandler(Tick);
            timer.Enabled = true;
        }

        static void Tick(object sender, ElapsedEventArgs args)
        {
            client.Tick();
        }
    }
}
