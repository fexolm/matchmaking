using System;
using System.IO;
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
                byte[] buffer;
                using (var m = new MemoryStream()) {
                    using (var writer = new BinaryWriter(m)) {
                        writer.Write($"{msgText}\t OK");
                    }
                    buffer = m.ToArray();
                }
                server.Send(new Packet(1, token, buffer), client);
            }));
            server.StartListener().Wait();
        }
    }
}