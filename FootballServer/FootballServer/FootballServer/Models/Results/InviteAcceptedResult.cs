using FootballServer.Enums;
using matchmaking;
using Newtonsoft.Json;

namespace FootballServer.Models.Results
{
    class InviteAcceptedResult : Result
    {
        [JsonProperty]
        public Token Token { get; set; }

        public InviteAcceptedResult(Token token, Player player) : base((int)MessageType.ACCEPT_INVITE, player)
        {
            Token = token;
        }
    }
}
