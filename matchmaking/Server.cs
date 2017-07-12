using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace matchmaking
{
	public class Server
	{
		private readonly TcpListener _listener;
		private readonly TcpClient _sender;
		List<Task> _connectionTasks = new List<Task>();
		object _lock = new Object();

		public Server(int port)
		{
			_listener = TcpListener.Create(port);
			_listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			_sender = new TcpClient(new IPEndPoint(IPAddress.Any, port));
			_sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		}

		public Task StartListener()
		{
			return Task.Run(async () =>
			{
				_listener.Start();
				while (true)
				{
					var tcpClient = await _listener.AcceptTcpClientAsync();
					Console.WriteLine("[Server] Client has connected");
					var task = StartHandleConnectionAsync(tcpClient);
					if (task.IsFaulted)
						task.Wait();
				}
			});
		}

		private async Task StartHandleConnectionAsync(TcpClient tcpClient)
		{
			var connectionTask = HandleConnectionAsync(tcpClient);
			lock (_lock)
				_connectionTasks.Add(connectionTask);
			try
			{
				await connectionTask;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				lock (_lock)
					_connectionTasks.Remove(connectionTask);
			}
		}

		private Task HandleConnectionAsync(TcpClient tcpClient)
		{
			return Task.Run(async () =>
			{
				using (var networkStream = tcpClient.GetStream())
				{
					while (tcpClient.Connected)
					{
						if (tcpClient.Available > 0)
						{
							var buffer = new byte[2048];
							var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
							await MessageArrived.Invoke(buffer, byteCount, tcpClient);
						}
					}
				}
			});
		}

		public event Func<byte[], int, TcpClient, Task> MessageArrived;

		public void Send(byte[] buffer, TcpClient client)
		{
			client.GetStream().Write(buffer, 0, buffer.Length);
		}
	}
}