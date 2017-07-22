using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using matchmaking;

namespace TestServer
{
    internal class Program
    {
        public static void Main(string[] args) {
            Server s = new Server(8001);
            s.AddHandler(1, (t, p, c) => Task.Run(() => {
                var str = $"1 {t} {p[0]}|";
                s.Send(Encoding.UTF8.GetBytes(str), c);
            }));
            s.StartListener().Wait();
        }
    }
}