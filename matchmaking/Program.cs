using System;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace matchmaking
{
    internal class Program
    {
        public static void Main(string[] args) {
            var server = new Server(8001);
            server.AddHandler(1, (token, stream, client) => Task.Run(() => {
                var msgText = stream.ReadString();
                Console.WriteLine(msgText);
                server.Send(new Packet(1, token, Encoding.UTF8.GetBytes($"{msgText}\t OK")), client);
            }));
            server.StartListener().Wait();
        }
    }
}