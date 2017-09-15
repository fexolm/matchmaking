﻿using System.Collections.Generic;
using HistoryBattlesServer.Rooms;

namespace HistoryBattlesServer.Requests
{
    public class LeaveRoomRequest : Request
    {
        public LeaveRoomRequest(HBPlayer player) : base((int) MessageType.LEAVE_ROOM, player) { }
        public override Result Valdate() {
            return !RoomManager.IsInRoom(Player)
                ? new Result("You are not in room")
                : Result.Ok;
        }

        public override Result Process() {
            RoomManager.LeaveRoom(Player);
            Player.OnLeave = null;
            return Result.Ok;
        }
    }
}