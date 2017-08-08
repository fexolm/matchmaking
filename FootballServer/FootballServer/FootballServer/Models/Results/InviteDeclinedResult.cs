using FootballServer.Enums;
using matchmaking;
using Newtonsoft.Json;

namespace FootballServer.Models.Results
{
    class InviteDeclinedResult : Result
    {
        [JsonProperty]
        public Token Token { get; set; }

        public InviteDeclinedResult(Invite invite) : base((int)InviteStatus.REJECTED, invite.From)
        {
        }
    }
}
