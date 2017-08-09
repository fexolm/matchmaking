using FootballClient.Enums;
using FootballClient.Models.Requests;
using FootballClient.Models.Results;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FootballClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client<Player>();
            client.Connect(IPAddress.Parse("127.0.0.1"), 8001);

            client.AddHandler((int)MessageType.CREATE_INVITE,
                (msg) => Task.Run(() =>
                {
                client.Send(new ValueRequest<Token>(Convert.ToInt32(MessageType.CREATE_INVITE),
                    client.Get(), client.Get().Token));
                }));
            client.AddHandler((int)MessageType.ACCEPT_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    client.Send(new ValueRequest<Token>((int)MessageType.ACCEPT_INVITE,
                        client.Get(), client.Get().Token));

                }));
            client.AddHandler((int)MessageType.DECLINE_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    client.Send(new ValueRequest<Token>((int)MessageType.DECLINE_INVITE,
                        client.Get(), client.Get().Token));
                    
                }));
            while (true)
            {
                try
                {
                    Console.Write("Write a command: ");
                    client._handlers[Convert.ToInt32(Console.ReadLine())]
                        (new Newtonsoft.Json.Linq.JObject());
                }
                catch
                {
                    Console.WriteLine("wrong command");
                }
            }
        }
    }
}
