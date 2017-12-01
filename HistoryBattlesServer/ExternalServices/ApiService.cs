﻿using System;
using HistoryBattlesServer.ExternalServices.Models;
using HistoryBattlesServer.Requests;
using HistoryBattlesServer.Rooms;
using matchmaking;
using StartGameRequest = HistoryBattlesServer.ExternalServices.Models.StartGameRequest;

namespace HistoryBattlesServer.ExternalServices
{
    public static class ApiService
    {
        //TODO: change master ip
        private const string MASTER_IP = "http://104.199.106.137:8000";

        //TODO: change private key
        private const string PRIVATE_KEY = "12345";

        //TODO: implement
        public static Result ValidateRoomParams(RoomParams roomParams, Player player) {
            var response = WebService.Post<dynamic, object>("http://35.195.58.247/api/mm", new {token = player.Token});
            if (roomParams.Bet < response.money && roomParams.Rang == response.rank &&
                roomParams.Fraction == response.fraction) {
                return Result.Ok;
            }
            return new Result("Validation room params error");
        }

        //TODO: test
        public static ValueResult<string> GetRemoteServerIp(Room room) {
            try {
                var responseData = WebService.Post<IpResponse, StartGameRequest>(
                    $"{MASTER_IP}/start_game/",
                    new StartGameRequest(PRIVATE_KEY, room));
                if (!responseData.success) {
                    return new ValueResult<string>(responseData.error);
                }
                return ValueResult<string>.BuildResult(true, responseData.ip);
            }
            catch (Exception ex) {
                return new ValueResult<string>(ex.ToString());
            }
        }
    }
}