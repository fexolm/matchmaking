using matchmaking;
using Newtonsoft.Json;

namespace HistoryBattlesServer
{
    public class HBPlayer : Player
    {
        [JsonIgnore]    
        public string RoomToken { get; set; }

        public HBPlayer() {
            RoomToken = string.Empty;
        }
    }
}