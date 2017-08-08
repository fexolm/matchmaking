using matchmaking;

namespace FootballServer.Models.Requests
{
    class TokenRequest : Request
    {
        public Token Token { get; set; }

        public TokenRequest(int id, Player player) : base(id, player)
        {
        }
    }
}
