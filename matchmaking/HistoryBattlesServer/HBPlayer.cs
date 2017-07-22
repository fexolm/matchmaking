using matchmaking;

namespace HistoryBattlesServer
{
    public class HBPlayer : Player
    {
        public string RoomToken { get; set; }

        public HBPlayer() {
            RoomToken = string.Empty;
        }
    }
}