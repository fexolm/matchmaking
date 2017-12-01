using System.Collections.Generic;
using HistoryBattlesServer.ExternalServices;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Requests
{
    public class CreateRoomRequest : Request
    {
        public RoomParams RoomParams { get; set; }

        public CreateRoomRequest(HBPlayer player) : base((int) MessageType.CREATE_ROOM, player) { }

        public override Result Valdate() {
            var validateRoomParamsResult = ApiService.ValidateRoomParams(RoomParams, Player);
            if (!validateRoomParamsResult.Success)
                return validateRoomParamsResult;

            return RoomManager.IsInRoom(Player)
                ? new Result("You are alrady in room")
                : Result.Ok;
        }

        public override Result Process() {
            RoomManager.CreateRoom(Player, RoomParams);
            Player.OnLeave = () => RoomManager.LeaveRoom(Player);
            return Result.Ok;
        }
    }
}