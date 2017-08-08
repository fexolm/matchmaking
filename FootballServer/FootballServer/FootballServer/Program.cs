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
                var request = msg.ToObject<TokenRequest>();
                if (server._players.ContainsKey(request.Token.ToString()))
                {
                    Invite inv = new Invite(Token.Generate(), player, server._players[request.Token.ToString()]);
                    _invites[inv.Token] = inv;
                    server.Send(new InviteRecievedResult(inv));
                }
                else
                {
                    server.Send(new ErrorResult("No such player on server", player));
                }
            }));
            server.AddHandler((int)MessageType.ACCEPT_INVITE,
                (player, msg, client) => Task.Run(() =>
                {
                    var request = msg.ToObject<TokenRequest>();
                    if (_invites.ContainsKey(request.Token))
                    {
                        _invites[request.Token].Status = InviteStatus.ACCEPTED;
                        server.Send(new InviteAcceptedResult(Token.Generate(), _invites[request.Token].From));
                    }
                    else
                    {
                        server.Send(new ErrorResult("Invite is not valid", player));
                    }
                }));
            server.AddHandler((int)MessageType.DECLINE_INVITE,
                (player, msg, client) => Task.Run(() =>
                {
                    var request = msg.ToObject<InviteDeclinedRequest>();
                    if (_invites.ContainsKey(request.Invite.Token))
                    {
                        _invites[request.Invite.Token].Status = InviteStatus.REJECTED;
                        server.Send(new InviteDeclinedResult(_invites[request.Invite.Token]));
                    }
                    else
                    {
                        server.Send(new ErrorResult("Invite is not valid", player));
                    }
                }));
            server.AddHandler((int)MessageType.REJECT_INVITE,
                (player, msg, client) => Task.Run(() =>
                {
                    var request = msg.ToObject<InviteDeclinedRequest>();
                    if (_invites.ContainsKey(request.Invite.Token))
                    {
                        _invites[request.Invite.Token].Status = InviteStatus.REJECTED;
                        server.Send(new InviteDeclinedResult(_invites[request.Invite.Token]));
                        server.Send(new InviteRejectedResult(_invites[request.Invite.Token]));
                    }
                    else
                    {
                        server.Send(new ErrorResult("Invite is not valid", player));
                    }
                }));
            server.StartListener().Wait();
        }
    }


}
