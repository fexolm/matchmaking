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

        public static void JoinRoom(HBPlayer player, string roomToken) {
            _rooms[roomToken].Other = player;
            playerJoined.Invoke(_rooms[roomToken].Owner, player);
            player.RoomToken = roomToken;
        }

        public static void LeaveRoom(HBPlayer player) {
            if (!IsInRoom(player)) return;
            var room = _rooms[player.RoomToken];
            if (IsOwner(player)) {
                if (RoomFull(player)) roomClosed.Invoke(room.Other);
                if (room.Other != null) room.Other.RoomToken = string.Empty;
                RemoveRoom(player.RoomToken);
            }
            else {
                opponentLeaved.Invoke(room.Owner);
                room.Other = null;
            }
            player.RoomToken = string.Empty;
        }

        public static Room RemoveRoom(string roomToken) {
            if (roomToken == null) return null;
            _rooms.TryRemove(roomToken, out Room removableRoom);
            return removableRoom;
        }

        public static bool IsInRoom(HBPlayer player) {
            if (player == null) return false;
            return player.RoomToken != string.Empty;
        }

        public static bool IsInRoom(HBPlayer player, string roomToken) {
            if (player?.RoomToken == null) return false;
            return player.RoomToken == roomToken;
        }

        public static bool IsOwner(HBPlayer player) {
            if (player == null || !_rooms.ContainsKey(player.RoomToken))
                return false;
            return _rooms[player.RoomToken].Owner == player;
        }

        public static bool RoomFull(HBPlayer player) {
            if (player?.RoomToken == null) return false;
            if (!_rooms.ContainsKey(player.RoomToken))
                return false;
            var room = _rooms[player.RoomToken];
            return room.Other != null;
        }

        public static Room GetRoomInfo(string roomToken) {
            if (roomToken == null) return null;
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