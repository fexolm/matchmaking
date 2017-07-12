using System;
using System.Net;

namespace matchmaking
{
    internal class Program
    {
        public static void Main(string[] args) {
            var server = new Server(8001);
            server.BeginListen();
            Server.MessageArrived += HandleMessage;
        }

        private static string HandleMessage(Tuple<string, IPEndPoint> msg) {
            return null;
        }
    }
}