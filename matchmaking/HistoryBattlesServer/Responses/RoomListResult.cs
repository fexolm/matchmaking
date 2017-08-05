using System.Collections.Generic;
using System.Linq;
using HistoryBattlesServer.Requests;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Responses
{
    public class RoomListResult : Response
    {
        public RoomListResult(HBPlayer player, Result result) : base((int) MessageType.SHOW_ROOMS, player, result) { }    
    }
}