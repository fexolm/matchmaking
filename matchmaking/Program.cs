using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace matchmaking
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var server = new Server(8001);
			server.MessageArrived += (buf, len, client) => Task.Run(() =>
			{
				var msgText = Encoding.UTF8.GetString(buf, 0, len);
				Console.WriteLine(msgText);
				server.Send(Encoding.UTF8.GetBytes($"{msgText}\t OK"), client);
			});
			server.StartListener().Wait();

		}
	}
}