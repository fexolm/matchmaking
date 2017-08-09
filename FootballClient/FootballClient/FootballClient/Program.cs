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
                (player, msg, response) => Task.Run(() =>
                {
                    client.Send(new ValueRequest<Token>((int)MessageType.CREATE_INVITE,
                        client.Get(), client.Get().Token));
                }));
            client.AddHandler((int)MessageType.ACCEPT_INVITE,
                (player, msg, response) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();

                }));
            client.AddHandler((int)MessageType.DECLINE_INVITE,
                (player, msg, response) => Task.Run(() =>
                {

                }));
            client.AddHandler((int)MessageType.REJECT_INVITE,
                (player, msg, response) => Task.Run(() =>
                {

                }));
        }
    }
}
