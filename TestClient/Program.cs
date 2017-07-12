using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    public static class Program
    {
        public static void Main(string[] args) {
            var tcpClient = new TcpClient(new IPEndPoint(IPAddress.Any, int.Parse(args[0])));
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000));
            while (true) {
                tcpClient.Client.Send(new Packet(1, "AGDFFE23423dsdf", Encoding.UTF8.GetBytes(Console.ReadLine()))
                    .Serialize());
                while (tcpClient.Available == 0) ;
                var stream = tcpClient.GetStream();
                using (var reader = new BinaryReader(stream)) {
                    reader.ReadInt32();
                    reader.ReadString();
                    Console.WriteLine(reader.ReadString());
                }
            }
        }
    }
}