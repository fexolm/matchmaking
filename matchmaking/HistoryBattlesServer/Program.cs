using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using HistoryBattlesServer.Requests;
using HistoryBattlesServer.Responses;
using HistoryBattlesServer.Rooms;
using matchmaking;

namespace HistoryBattlesServer
{
    internal class Program
    {
        public static void Main(string[] args) {
            var s = new Server<HBPlayer>(8001);

            s.AddHandler((int) MessageType.CREATE_ROOM, (player, parameters, client) => Task.Run(() => {
                var request = new CreateRoomRequest(player);
                request.Deserialize(parameters);
                var validationResult = request.Valdate();
                if (validationResult.Success) {
                    request.Process();
                }
                s.Send(new Response((int) MessageType.CREATE_ROOM, player, validationResult));
            }));
            s.AddHandler((int) MessageType.JOIN_ROOM, (player, parameters, client) => Task.Run(() => {
                var request = new JoinRoomRequest(player);
                request.Deserialize(parameters);
                var validationResult = request.Valdate();
                if (validationResult.Success) {
                    request.Process();
                }
                s.Send(new Response((int) MessageType.JOIN_ROOM, player, validationResult));
            }));
            s.AddHandler((int) MessageType.LEAVE_ROOM, (player, parameters, client) => Task.Run(() => {
                var request = new LeaveRoomRequest(player);
                request.Deserialize(parameters);
                var validationResult = request.Valdate();
                if (validationResult.Success) {
                    request.Process();
                }
                s.Send(new Response((int) MessageType.LEAVE_ROOM, player, validationResult));
            }));
            s.AddHandler((int) MessageType.START_GAME, (player, parameters, client) => Task.Run(() => {
                var request = new StartGameRequest(player);
                request.Deserialize(parameters);
                var validationResult = request.Valdate();
                if (validationResult.Success) {
                    var processResult = request.Process();
                    if (processResult.Success) {
                        var room = RoomManager.GetRoomInfo(player.RoomToken);
                        s.Send(new StartGameResponse(room.Owner, processResult));
                        s.Send(new StartGameResponse(room.Other, processResult));
                    }
                    else {
                        s.Send(new StartGameResponse(player, processResult));
                    }
                }
                else {
                    s.Send(new StartGameResponse(player, validationResult));
                }
            }));
            s.AddHandler((int) MessageType.SHOW_ROOMS, (player, parameters, client) => Task.Run(() => {
                var request = new ShowRoomsRequest(player);
                request.Deserialize(parameters);
                var validationResult = request.Valdate();
                if (validationResult.Success) {
                    var processResult = request.Process();
                    s.Send(new RoomListResult(player, processResult));
                }
                else {
                    s.Send(new RoomListResult(player, validationResult));
                }
            }));

            RoomManager.opponentLeaved = (owner) => {
                s.Send(new Message<HBPlayer>((int) MessageType.OPPONENT_LEAVED, owner));
            };

            RoomManager.playerJoined = (owner, oponent) => {
                s.Send(new Message<HBPlayer>((int) MessageType.PLAYER_JOINED, owner));    
            };
            RoomManager.roomClosed = (opponent) => {
                s.Send(new Message<HBPlayer>((int) MessageType.ROOM_CLOSED, opponent));
            };
            s.StartListener().Wait();
        }
    }
}