using System.Collections.Generic;
using HistoryBattlesServer.ExternalServices;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Requests
{
    public class StartGameRequest : Request
    {
        public StartGameRequest(HBPlayer player) : base((int) MessageType.START_GAME, player) { }

        public override Result Valdate() {
            if (!RoomManager.IsInRoom(Player)) {
                return new Result("You are not in room");
            }
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!RoomManager.RoomFull(Player)) {
                return new Result("Room is not full");
            }
            return Result.Ok;
        }

        public override Result Process() {
            var masterResponse = ApiService.GetRemoteServerIp(RoomManager.GetRoomInfo(Player.RoomToken));
            return masterResponse;
        }
    }
}