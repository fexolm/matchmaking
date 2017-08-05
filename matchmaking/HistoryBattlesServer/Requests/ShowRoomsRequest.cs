using System.Collections.Generic;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Requests
{
    public class ShowRoomsRequest : Request
    {
        public ShowRoomsRequest(HBPlayer player) : base((int)MessageType.SHOW_ROOMS, player) { }
      
        public override Result Valdate() {
            return RoomManager.IsInRoom(Player)
                ? new Result("You are in room")
                : Result.Ok;
        }

        public override Result Process() {
            var result = new ValueResult<IEnumerable<Room>>(RoomManager.GetEmptyRooms());
            return result;
        }
    }
}