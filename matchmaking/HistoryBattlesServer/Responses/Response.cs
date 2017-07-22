using System;
using HistoryBattlesServer.Requests;
using matchmaking;

namespace HistoryBattlesServer.Responses
{
    public class Response : Message<HBPlayer>
    {
        protected Result Result;

        public Response(int id, HBPlayer player, Result result) : base(id, player) {
            Result = result;
        }

        protected virtual string SerializeBody() {
            return string.Empty;
        }

        public override string Serialize() {
            if (Result.Success) {
                return $"{base.Serialize()} OK {SerializeBody()}";
            }
            return $"{base.Serialize()} ERROR {Result.ErrorMessage}";
        }
    }
}