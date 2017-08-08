using FootballServer.Enums;
using Newtonsoft.Json;

namespace FootballServer.Models.Results
{
    class InviteRejectedResult : Result
    {
        [JsonProperty]
        public Token Token { get; set; }

        public InviteRejectedResult(Invite invite) : base((int)InviteStatus.REJECTED, invite.To)
        {

        }
    }
}
