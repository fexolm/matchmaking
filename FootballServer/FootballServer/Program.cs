using FootballServer.Enums;
using matchmaking;
using System.Threading.Tasks;
using FootballServer.Models.Requests;
using FootballServer.Models.Results;
using System.Collections.Concurrent;

namespace FootballServer
{
    class Program
    {
        private static ConcurrentDictionary<Token, Invite> _invites =
            new ConcurrentDictionary<Token, Invite>();

        static void Main(string[] args)
        {
            var server = new Server<Player>(8001);

            server.AddHandler((int)MessageType.CREATE_INVITE,
                (player, msg, client) => Task.Run(() =>
            {
                var request = msg.ToObject<ValueRequest<Token>>();
                if (server._players.ContainsKey(request.Value.ToString()))
                {
                    var inv = new Invite(Token.Generate(), player, server._players[request.Value.ToString()]);
                    _invites[inv.Token] = inv;
                    server.Send(new ValueResult<Invite>((int)MessageType.RECIEVED_INVITE, inv.To, inv));
                }
                else
                {
                    server.Send(new ErrorResult("No such player on server", player));
                }
            }));
            server.AddHandler((int)MessageType.ACCEPT_INVITE,
                (player, msg, client) => Task.Run(() =>
                {
                    var request = msg.ToObject<ValueRequest<Token>>();
                    if (_invites.ContainsKey(request.Value))
                    {
                        _invites[request.Value].Status = InviteStatus.ACCEPTED;
                        var inv = _invites[request.Value];
                        var token = Token.Generate();
                        server.Send(new ValueResult<Token>((int)MessageType.ACCEPT_INVITE,
                            inv.To, token));
                        server.Send(new ValueResult<Token>((int)MessageType.ACCEPT_INVITE,
                            inv.From, token));
                        _invites.TryRemove(request.Value, out inv);
                    }
                    else
                    {
                        server.Send(new ErrorResult("Invite is not valid", player));
                    }
                }));
            server.AddHandler((int)MessageType.DECLINE_INVITE,
                (player, msg, client) => Task.Run(() =>
                {
                    var request = msg.ToObject<ValueRequest<Invite>>();
                    if (_invites.ContainsKey(request.Value.Token))
                    {
                        _invites[request.Value.Token].Status = InviteStatus.REJECTED;
                        var inv = _invites[request.Value.Token];
                        server.Send(new ValueResult<Invite>((int)MessageType.REJECT_INVITE,
                            inv.From, inv));
                        _invites.TryRemove(request.Value.Token, out inv);
                    }
                    else
                    {
                        server.Send(new ErrorResult("Invite is not valid", player));
                    }
                }));
            server.AddHandler((int)MessageType.REJECT_INVITE,
                (player, msg, client) => Task.Run(() =>
                {
                    var request = msg.ToObject<ValueRequest<Invite>>();
                    if (_invites.ContainsKey(request.Value.Token))
                    {
                        _invites[request.Value.Token].Status = InviteStatus.REJECTED;
                        var inv = _invites[request.Value.Token];
                        server.Send(new ValueResult<Invite>((int)MessageType.ACCEPT_INVITE,
                            inv.From, inv));
                        server.Send(new ValueResult<Invite>((int)MessageType.ACCEPT_INVITE,
                            inv.To, inv));
                        _invites.TryRemove(request.Value.Token, out inv);
                    }
                    else
                    {
                        server.Send(new ErrorResult("Invite is not valid", player));
                    }
                }));
            System.Console.WriteLine("starting to listen...");
            server.StartListener().Wait();
            System.Console.WriteLine("end");
        }
    }


}