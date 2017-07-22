using System.Collections.Generic;
using HistoryBattlesServer.ExternalServices;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Requests
{
    public class JoinRoomRequest : Request
    {
        private string _roomToken;
        public JoinRoomRequest(HBPlayer player) : base((int)MessageType.JOIN_ROOM, player) { }

        public override void Deserialize(List<string> parameters) {
            _roomToken = parameters[0];
        }

        public override Result Valdate() {
            var room = RoomManager.GetRoomInfo(_roomToken);
            if (room == null) {
                return new Result("Room not exist");
            }
            var validateRoomParamsResult = ApiService.ValidateRoomParams(room.Params, Player);
            if (!validateRoomParamsResult.Success)
                return validateRoomParamsResult;
            return RoomManager.IsInRoom(Player)
                ? new Result("You are alrady in room")
                : Result.Ok;
        }

        public override Result Process() {
            RoomManager.JoinRoom(Player, _roomToken);
            return Result.Ok;
        }
    }
}