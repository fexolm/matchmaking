using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace matchmaking
{
    public class Server<TPlayer>
        where TPlayer : Player, new()
    {
        public ConcurrentDictionary<string, TPlayer> _players =
            new ConcurrentDictionary<string, TPlayer>();
        
        public delegate Task Handler(TPlayer player, List<string> parameters, TcpClient client);

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
                                    Debug.Assert(_handlers.ContainsKey(1), $"_hanlers has key {1}");
                                    var buffer = new byte[available];
                                    networkStream.Read(buffer, 0, buffer.Length);
                                    var msg = Encoding.UTF8.GetString(buffer).TrimEnd('|');
                                    var parameters = msg.Split(' ').ToList();
                                    var id = int.Parse(parameters[0]);
                                    var token = parameters[1];
                                    TPlayer player;
                                    if (_players.ContainsKey(token)) {
                                        player = _players[token];
                                    }
                                    else {
                                        player = new TPlayer {
                                            Token = token,
                                            Client = tcpClient
                                        };
                                    }
                                    parameters.RemoveAt(0);
                                    parameters.RemoveAt(0);
                                    await _handlers[id].Invoke(player, parameters, tcpClient);
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

        public void AddHandler(int id, Handler handler) {
            _handlers.Add(id, handler);
        }

        public void Send(Message<TPlayer> msg) {
            var buffer = Encoding.UTF8.GetBytes($"{msg.Serialize()}|");
            msg.Player.Client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}