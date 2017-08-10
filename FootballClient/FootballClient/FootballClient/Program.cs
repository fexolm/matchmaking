using System.Net;

namespace FootballClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = Token.Generate();
            MMClient Ars = new MMClient(token);
            Ars.Connect(IPAddress.Parse("127.0.0.1"), 8001);
            int id = System.Convert.ToInt32(System.Console.ReadLine());
            Ars.Send(id, token);

            Ars.OnInviteError = System.Console.WriteLine;
            Ars.OnInviteAccepted = System.Console.WriteLine;
            Ars.OnInviteRejected = System.Console.WriteLine;
            Ars.OnInviteRecieved = System.Console.WriteLine;

            while (true)
            {
                System.Threading.Thread.Sleep(100);
                Ars.Tick();
            }
        }
    }
}
