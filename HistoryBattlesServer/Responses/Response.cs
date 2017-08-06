using HistoryBattlesServer.Requests;
using matchmaking;

namespace HistoryBattlesServer.Responses
{
    public class Response : Message<HBPlayer>
    {
        public Result Result { get; set; }

        public Response(int id, HBPlayer player, Result result) : base(id, player) {
            Result = result;
        }
    }
}