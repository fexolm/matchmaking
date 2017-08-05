using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace matchmaking
{
    public class Server<TPlayer>
        where TPlayer : Player, new()
    {
        public ConcurrentDictionary<string, TPlayer> _players =
            new ConcurrentDictionary<string, TPlayer>();

        public delegate Task Handler(TPlayer player, Message<TPlayer> msg, TcpClient client);

        private readonly TcpListener _listener;
        private readonly object _lock = new object();

        private readonly Dictionary<int, Handler> _handlers = new Dictionary<int, Handler>();

        public Server(int port) {
            _listener = TcpListener.Create(port);
            _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public Task StartListener() {
            return Task.Run(async () => {
                _listener.Start();
                while (true) {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    Console.WriteLine("[Server] Client has connected");
                    var task = StartHandleConnectionAsync(tcpClient);
                    if (task.IsFaulted)
                        task.Wait();
                }
            });
        }

        private async Task StartHandleConnectionAsync(TcpClient tcpClient) {
            var connectionTask = HandleConnectionAsync(tcpClient);
            try {
                await connectionTask;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        private Task HandleConnectionAsync(TcpClient tcpClient) {
            return Task.Run(async () => {
                    using (var networkStream = tcpClient.GetStream()) {
                        try {
                            while (tcpClient.Connected) {
                                int available;
                                while ((available = tcpClient.Available) > 0) {
                                    var buffer = new byte[available];
                                    networkStream.Read(buffer, 0, buffer.Length);
                                    var fullMsg = Encoding.UTF8.GetString(buffer);
                                    while (!string.IsNullOrEmpty(fullMsg)) {
                                        var msg = ReadOneJson(ref fullMsg);
                                        var msgObj = JsonConvert.DeserializeObject<Message<TPlayer>>(msg);
                                        TPlayer player;
                                        if (_players.ContainsKey(msgObj.Player.Token)) {
                                            player = _players[msgObj.Player.Token];
                                        }
                                        else {
                                            player = new TPlayer {
                                                Token = msgObj.Player.Token,
                                                Client = tcpClient
                                            };
                                            _players[msgObj.Player.Token] = player;
                                        }
                                        Debug.Assert(_handlers.ContainsKey(msgObj.Id), $"_hanlers has key {msgObj.Id}");
                                        await _handlers[msgObj.Id].Invoke(player, msgObj, tcpClient);
                                    }
                                }
                            }
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            );
        }

        private string ReadOneJson(ref string str) {
            int bracketsCount = 0;
            int i = 0;
            do {
                if (str[i] == '{') bracketsCount++;
                if (str[i] == '}') bracketsCount--;
                i++;
            } while (bracketsCount > 0);
            var res = str.Substring(0, i);
            str = i == str.Length
                ? string.Empty
                : str.Substring(i, str.Length);
            return res;
        }

        public void AddHandler(int id, Handler handler) {
            _handlers.Add(id, handler);
        }

        public void Send(Message<TPlayer> msg) {
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
            msg.Player.Client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}