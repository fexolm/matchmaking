using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using matchmaking;

namespace FootballServer.Models.Requests
{
    class ValueRequest<TValue> : Request
    {
        public TValue Value { get; set; }

        // id - message type
        // player - reciever
        public ValueRequest(int id, Player player, TValue value) : base(id, player)
        {
            Value = value;
        }
    }
}
