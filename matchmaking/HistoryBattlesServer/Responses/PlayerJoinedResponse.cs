using HistoryBattlesServer.Requests;

namespace HistoryBattlesServer.Responses
{
    public class PlayerJoinedResponse : Response
    {
        private readonly string _opponentToken;

        public PlayerJoinedResponse(HBPlayer player, Result result, string opponentToken) : base(
            (int) MessageType.PLAYER_JOINED, player,
            result) {
            _opponentToken = opponentToken;
        }

        protected override string SerializeBody() {
            return _opponentToken;
        }
    }
}