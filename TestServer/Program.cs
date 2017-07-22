using matchmaking;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace TestServer
{

	class Program
	{
		private static ConcurrentDictionary<string, TcpClient> _players;

		private enum MessageType: int
		{
			TEST,
			START_GAME,
			LEAVE_GAME,
			CREATE_ROOM,
			DO_WHATEVER_YOU_WANT,
		}

		public static void Main(string[] args)
		{
			_players = new ConcurrentDictionary<string, TcpClient>();
			var server = new Server(8001);
			server.AddHandler((int)MessageType.TEST, (token, stream, client) => Task.Run(() => {
				var msgText = stream.ReadString();
				Console.WriteLine(msgText);
				byte[] buffer;
				using (var m = new MemoryStream()) {
					using (var writer = new BinaryWriter(m)) {
						writer.Write($"{msgText}\t OK");
					}
					buffer = m.ToArray();
				}
				server.Send(new Packet((int)MessageType.TEST, token, buffer), client);
			}));

			server.AddHandler((int)MessageType.START_GAME, (token, stream, client) => Task.Run(() => {

				_players.AddOrUpdate(token, client, (t, old) => client);
				//server response  is not really needed, but why not!?
				//you may not add it in your program
				server.Send(new Packet(1, token, new byte[] { 1 }), client);
			}));

			server.AddHandler((int)MessageType.LEAVE_GAME, (token, stream, client) => Task.Run(() => {
				TcpClient remove;
				_players.TryRemove(token, out remove);

				//it's not perfect and would cause (handled) exceptions.. in nerly future i ll fix it
				remove.Close();
				
				server.Send(new Packet(1, token, new byte[] { 1 }), client);
			}));

			server.StartListener().Wait();
		}
	}
}
