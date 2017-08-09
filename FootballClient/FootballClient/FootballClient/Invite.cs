using FootballClient.Enums;
using Newtonsoft.Json;

namespace FootballClient
{
    class Invite
    {
        [JsonProperty]
        public readonly Token Token;

        [JsonProperty]
        public readonly Player From;

        [JsonProperty]
        public readonly Player To;

        [JsonIgnore]
        public InviteStatus Status { get; set; }

        public Invite(Token token, Player frm, Player to)
        {
            Token = token;
            From = frm;
            To = to;
            Status = InviteStatus.PENDING;
        }
    }
}
