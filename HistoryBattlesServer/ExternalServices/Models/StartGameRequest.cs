using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.ExternalServices.Models
{
    public class StartGameRequest
    {
        public string key { get; set; }

        public Room body { get; set; }

        public StartGameRequest(string key, Room room) {
            this.key = key;
            body = room;
        }
    }
}