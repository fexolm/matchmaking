using System.Collections.Generic;
using System.ComponentModel;
using HistoryBattlesServer.ExternalServices;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Requests
{
    public class CreateRoomRequest : Request
    {
        private bool deserializationfailed = false;

        private readonly RoomParams _roomParams = new RoomParams();

        public CreateRoomRequest(HBPlayer player) : base((int) MessageType.CREATE_ROOM, player) { }

        public override void Deserialize(List<string> parameters) {
            try {
                _roomParams.Deserialize(parameters);
            }
            catch {
                deserializationfailed = true;
            }
        }

        public override Result Valdate() {
            if (deserializationfailed) {
                return new Result("params deserialization failed");
            }
            var validateRoomParamsResult = ApiService.ValidateRoomParams(_roomParams, Player);
            if (!validateRoomParamsResult.Success)
                return validateRoomParamsResult;

            return RoomManager.IsInRoom(Player)
                ? new Result("You are alrady in room")
                : Result.Ok;
        }

        public override Result Process() {
            RoomManager.CreateRoom(Player, _roomParams);
            return Result.Ok;
        }
    }
}