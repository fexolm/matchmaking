using FootballServer.Enums;
using Newtonsoft.Json;

namespace FootballServer.Models.Requests
{
    class InviteDeclinedRequest : Request
    {
        [JsonProperty]
        public Invite Invite { get; set; }

        public InviteDeclinedRequest(Invite invite) : base((int)InviteStatus.REJECTED, invite.From)
        {
            Invite = invite;
        }
    }
}
