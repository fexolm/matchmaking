using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FootballClient
{
    class Client
    {
        private readonly TcpClient _client;

        public delegate Task Handler(JObject msg);

        public readonly Dictionary<int, Handler> _handlers =
            new Dictionary<int, Handler>();

        public Client()
        {
            _client = new TcpClient();
        }

        public void Tick()
        {
            try
            {
                var networkStream = _client.GetStream();
                int available;
                while ((available = _client.Available) > 0)
                {
                    var buffer = new byte[available];
                    networkStream.Read(buffer, 0, buffer.Length);
                    var fullMsg = Encoding.UTF8.GetString(buffer);
                    while (!string.IsNullOrEmpty(fullMsg))
                    {
                        var msg = ReadOneJson(ref fullMsg);
                        var msgObj = JObject.FromObject(JsonConvert.DeserializeObject(msg));
                        var m = msgObj.ToObject<Message>();
                        _handlers[m.Id].Invoke(msgObj);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Connect(IPAddress ip, int port)
        {
            try
            {
                _client.Client.Connect(new IPEndPoint(ip, port));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public void AddHandler(int id, Handler handler)
        {
            _handlers.Add(id, handler);
        }

        public void Send(Message msg)
        {
            var m = JsonConvert.SerializeObject(msg);
            var buffer = Encoding.UTF8.GetBytes(m);
            _client.GetStream().Write(buffer, 0, buffer.Length);
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

        ~Client()
        {
            Disconnect();
            _client.Close();
            _handlers.Clear();
        }
    }
}