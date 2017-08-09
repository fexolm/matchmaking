using FootballClient.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FootballClient
{
    class Client<TPlayer>
        where TPlayer : Player, new()
    {
        private Player _client { get; set; }

        public delegate Task Handler(JObject msg);

        public readonly Dictionary<int, Handler> _handlers = new Dictionary<int, Handler>();

        public Client()
        {
            _client = new Player();
            _client.Client = new TcpClient();
            _client.Token = Token.Generate();

            int available;
            if ((available = _client.Client.Available) > 0)
            {
                var networkStream = _client.Client.GetStream();
                var buffer = new byte[available];
                networkStream.Read(buffer, 0, buffer.Length);
                var fullMsg = Encoding.UTF8.GetString(buffer);
                while (!string.IsNullOrEmpty(fullMsg))
                {
                    var msg = ReadOneJson(ref fullMsg);
                    var msgObj = JObject.FromObject(JsonConvert.DeserializeObject(msg));
                }
            }
        }

        public void Connect(IPAddress ip, int port)
        {

            _client.Client.Connect(new IPEndPoint(ip, port));
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
            _client.Client.GetStream().Write(buffer, 0, buffer.Length);
        }

        public Player Get()
        {
            return _client;
        }
    }
}
