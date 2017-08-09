using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FootballClient
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Client<TPlayer>
    {
        private TcpClient _client { get; set; }

        public delegate Task Handler(TPlayer player, JObject msg, TcpClient client);

        private readonly Dictionary<int, Handler> _handlers = new Dictionary<int, Handler>();

        public Client()
        {
            _client = new TcpClient();

            while (true)
            {
                var message = GetMessage();
                var msg = ReadOneJson(ref message);
                var msgObj = JObject.FromObject(JsonConvert.DeserializeObject(msg));
            }
        }

        public void Connect(int port)
        {
            
            _client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            var networkStream = _client.GetStream();
        }

        //?
        public static string GetMessage()
        {
            return "";
        }

        private static string ReadOneJson(ref string str)
        {
            str.Trim('\n');
            int bracketsCount = 0;
            int i = 0;
            do
            {
                if (str[i] == '{') bracketsCount++;
                if (str[i] == '}') bracketsCount--;
                i++;
            } while (bracketsCount > 0);
            var res = str.Substring(0, i);
            str = i == str.Length
                ? string.Empty
                : str.Substring(i, str.Length - i);
            if (!str.Contains("{"))
            {
                str = string.Empty;
            }
            return res;
        }

        public void AddHandler(int id, Handler handler)
        {
            _handlers.Add(id, handler);
        }

        public void Send(Message<TPlayer> msg)
        {
            var m = JsonConvert.SerializeObject(msg);
            var buffer = Encoding.UTF8.GetBytes(m);
            msg.Player.Client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
