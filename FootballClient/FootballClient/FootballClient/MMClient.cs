using FootballClient.Enums;
using FootballClient.Models.Requests;
using FootballClient.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballClient
{
    class MMClient : Client
    {
        public MMClient(Token token) : base(token)
        {
            AddHandler((int)MessageType.CREATE_INVITE,
                (msg) => Task.Run(() =>
                {
                    Send(new ValueRequest<Token>((int)MessageType.CREATE_INVITE,
                        new Player(_token, _client), _token));
                }));
            AddHandler((int)MessageType.ACCEPT_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    Send(new ValueRequest<Token>((int)MessageType.ACCEPT_INVITE,
                        new Player(_token, _client), _token));

                }));
            AddHandler((int)MessageType.DECLINE_INVITE,
                (msg) => Task.Run(() =>
                {
                    var result = msg.ToObject<ValueResult<Invite>>();
                    Send(new ValueRequest<Token>((int)MessageType.DECLINE_INVITE,
                        new Player(_token, _client), _token));

                }));
        }
    }
}
