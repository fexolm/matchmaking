using FootballServer.Enums;
using Newtonsoft.Json;

namespace FootballServer.Models.Results
{
    class InviteRecievedResult : Result
    {
        [JsonProperty]
        public Invite Invite { get; set; }

        public InviteRecievedResult(Invite invite) : base((int)MessageType.RECIEVED_INVITE, invite.To)
        {
            Invite = invite;
        }
    }
}
