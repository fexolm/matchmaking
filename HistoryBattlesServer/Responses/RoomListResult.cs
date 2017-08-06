using System.Collections.Generic;
using System.Linq;
using HistoryBattlesServer.Requests;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Responses
{
	public class RoomListResult : Response
	{
		public int Count {
			get {
				return ((ValueResult<IEnumerable<Room>>)Result).Value.Count();
			}
		}
		public RoomListResult(HBPlayer player, Result result) : base((int)MessageType.SHOW_ROOMS, player, result) { }
	}
}