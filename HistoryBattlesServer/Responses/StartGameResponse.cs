using HistoryBattlesServer.Requests;

namespace HistoryBattlesServer.Responses
{
    public class StartGameResponse : Response
    {
        public StartGameResponse(HBPlayer player, Result result) :
            base((int) MessageType.START_GAME, player, result) { }
    }
}