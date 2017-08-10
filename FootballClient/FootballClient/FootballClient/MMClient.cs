using FootballClient.Enums;
using FootballClient.Models.Results;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FootballClient
{
    class MMClient : Client
    {
        private ConcurrentDictionary<Player, Invite> _invites =
            new ConcurrentDictionary<Player, Invite>();

        public Action<Invite> OnInviteRecieved;

        public Action<Invite> OnInviteRejected;

        public Action<Invite> OnInviteAccepted;

        public Action<string> OnInviteError;

        public MMClient(Token token) : base(token)
        {
            AddHandler((int)MessageType.RECIEVED_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    OnInviteRecieved(result.Value);
                }));
            AddHandler((int)MessageType.ACCEPT_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    OnInviteAccepted(result.Value);
                }));
            AddHandler((int)MessageType.DECLINE_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    OnInviteRejected(result.Value);
                }));
            AddHandler((int)MessageType.ERROR,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ErrorResult>();
                    OnInviteError(result.ErrorMessage);
                }));
        }

        public void CreateInvite(Token token)
        {
            Send((int)MessageType.CREATE_INVITE, _token);
        }

        public void AcceptInvite(Invite invite)
        {
            Send((int)MessageType.ACCEPT_INVITE, invite);
        }

        public void DeclineInvite(Invite invite)
        {
            Send((int)MessageType.DECLINE_INVITE, invite);
        }
    }
}
