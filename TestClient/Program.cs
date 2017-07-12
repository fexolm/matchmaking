using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    public static class Program
    {
        public static void Main(string[] args) {
            var tcpClient = new TcpClient(new IPEndPoint(IPAddress.Any, int.Parse(args[0])));
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000));
            while (true) {
                tcpClient.Client.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
                while (tcpClient.Available == 0) ;
                var stream = tcpClient.GetStream();
                var buffer = new byte[tcpClient.Available];
                var msglen = stream.Read(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.UTF8.GetString(buffer));
            }
        }
    }
}