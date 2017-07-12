using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace matchmaking
{
    public class Server
    {
        public delegate Task Handler(string token, BinaryReader reader, TcpClient client);

        private readonly TcpListener _listener;
        private readonly List<Task> _connectionTasks = new List<Task>();
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
            lock (_lock)
                _connectionTasks.Add(connectionTask);
            try {
                await connectionTask;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            finally {
                lock (_lock)
                    _connectionTasks.Remove(connectionTask);
            }
        }

        private Task HandleConnectionAsync(TcpClient tcpClient) {
            return Task.Run(async () => {
                using (var networkStream = tcpClient.GetStream()) {
                    try {
                        while (tcpClient.Connected) {
                            while (tcpClient.Available > 0) {
                                var stream = tcpClient.GetStream();
                                using (var reader = new BinaryReader(stream)) {
                                    int msgType = reader.ReadInt32();
                                    string token = reader.ReadString();
                                    Debug.Assert(_handlers.ContainsKey(msgType), $"_hanlers has key {msgType}");
                                    await _handlers[msgType].Invoke(token, reader, tcpClient);
                                }
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }
                }
            });
        }

        public void AddHandler(int id, Handler handler) {
            _handlers.Add(id, handler);
        }

        public void Send(Packet packet, TcpClient client) {
            var buffer = packet.Serialize();
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}