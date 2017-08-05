using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace HistoryBattlesServer.Rooms
{
    public static class RoomManager
    {
        private static readonly ConcurrentDictionary<string, Room> _rooms
            = new ConcurrentDictionary<string, Room>();

        public static void CreateRoom(HBPlayer player, RoomParams roomParams) {
            _rooms.TryAdd(player.Token, new Room(player, roomParams));
            player.RoomToken = player.Token;
        }

        public static void CloseRoom(HBPlayer player) {
            Room room;
            _rooms.TryRemove(player.Token, out room);
            roomClosed.Invoke(room.Other);
            player.RoomToken = string.Empty;
        }

        public static void JoinRoom(HBPlayer player, string roomToken) {
            _rooms[roomToken].Other = player;
            playerJoined.Invoke(_rooms[roomToken].Owner, player);
            player.RoomToken = roomToken;
        }

        public static void LeaveRoom(HBPlayer player) {
            var room = _rooms[player.RoomToken];
            room.Other = null;
            opponentLeaved.Invoke(room.Owner);
            room.Owner.RoomToken = string.Empty;
            if (room.Other != null) {
                room.Other.RoomToken = string.Empty;
            }
        }

        public static Room RemoveRoom(string roomToken) {
            Room removableRoom;
            _rooms.TryRemove(roomToken, out removableRoom);
            return removableRoom;
        }

        public static bool IsInRoom(HBPlayer player) {
            return player.RoomToken != string.Empty;
        }

        public static bool IsInRoom(HBPlayer player, string roomToken) {
            return player.RoomToken == roomToken;
        }

        public static bool IsOwner(HBPlayer player) {
            if (!_rooms.ContainsKey(player.RoomToken))
                return false;
            return _rooms[player.RoomToken].Owner == player;
        }

        public static bool RoomFull(HBPlayer player) {
            if (!_rooms.ContainsKey(player.RoomToken))
                return false;
            var room = _rooms[player.RoomToken];
            return room.Other != null;
        }

        public static Room GetRoomInfo(string roomToken) {
            return _rooms.ContainsKey(roomToken)
                ? _rooms[roomToken]
                : null;
        }

        public static IEnumerable<Room> GetEmptyRooms() {
            return _rooms.Where(r => r.Value.Other == null).Select(r => r.Value);
        }

        public static Action<HBPlayer, HBPlayer> playerJoined;
        public static Action<HBPlayer> opponentLeaved;
        public static Action<HBPlayer> roomClosed;
    }
}