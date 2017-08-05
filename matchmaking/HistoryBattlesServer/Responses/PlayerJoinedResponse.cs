using HistoryBattlesServer.Requests;

namespace HistoryBattlesServer.Responses
{
    public class PlayerJoinedResponse : Response
    {
        public string OpponentToken { get; set; }

        public PlayerJoinedResponse(HBPlayer player, Result result, string opponentToken) : base(
            (int) MessageType.PLAYER_JOINED, player,
            result) {
            OpponentToken = opponentToken;
        }
    }
}