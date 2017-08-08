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
                if (server._players.ContainsKey(request.Token.ToString()))
                {
                    Invite inv = new Invite(Token.Generate(), player, server._players[request.Token.ToString()]);
                    _invites[inv.Token] = inv;
                    server.Send(new ValueResult<Invite>((int)MessageType.CREATE_INVITE, inv.To, inv));
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
                    if (_invites.ContainsKey(request.Token))
                    {
                        _invites[request.Token].Status = InviteStatus.ACCEPTED;
                        server.Send(new ValueResult<Token>((int)InviteStatus.ACCEPTED,
                            _invites[request.Token].To, Token.Generate()));
                        server.Send(new ValueResult<Token>((int)InviteStatus.ACCEPTED,
                            _invites[request.Token].From, Token.Generate()));
                        _invites.TryRemove(request.Token, _invites[request.Token]);
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
                    if (_invites.ContainsKey(request.Invite.Token))
                    {
                        _invites[request.Invite.Token].Status = InviteStatus.REJECTED;
                        server.Send(new ValueResult<Invite>((int)InviteStatus.REJECTED,
                            _invites[request.Invite.Token].From, _invites[request.Invite.Token]));
                        _invites.TryRemove(request.Token, _invites[request.Token]);
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
                    if (_invites.ContainsKey(request.Invite.Token))
                    {
                        _invites[request.Invite.Token].Status = InviteStatus.REJECTED;
                        server.Send(new ValueResult<Invite>((int)InviteStatus.REJECTED,
                            _invites[request.Invite.Token].From, _invites[request.Invite.Token]));
                        server.Send(new ValueResult<Invite>((int)InviteStatus.REJECTED,
                            _invites[request.Invite.Token].To, _invites[request.Invite.Token]));
                        _invites.TryRemove(request.Token, _invites[request.Token]);
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
