using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace matchmaking
{
    public class Server
    {
        public delegate Task Handler(string token, string msg, TcpClient client);

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
                                    var msg = Encoding.UTF8.GetString(buffer);
                                    await _handlers[1].Invoke("123", msg.TrimEnd('|'), tcpClient);
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

        public void Send(byte[] buffer, TcpClient client) {
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}