using HistoryBattlesServer.Requests;
using HistoryBattlesServer.Rooms;
using matchmaking;

namespace HistoryBattlesServer.ExternalServices
{
    //because ValueResult<string> is not possible
    public class IpString
    {
        public readonly string Ip;

        public IpString(string ip) {
            Ip = ip;
        }
    }

    public static class ApiService
    {
        //TODO: change master ip
        private const string MASTER_IP = "192.168.0.1";

        //TODO: implement
        public static Result ValidateRoomParams(RoomParams roomParams, Player player) {
            return Result.Ok;
        }

        //TODO: implement
        public static ValueResult<IpString> GetRemoteServerIp(Room room) {
            var response = WebService.Post(MASTER_IP, GetJsonromRoom(room));
            if (IsSuccess(response)) {
                return new ValueResult<IpString>(GetIpFromResponse(response));
            }
            return new ValueResult<IpString>(GetErrorFromResponse(response));
        }

        //TODO: implement
        private static string GetJsonromRoom(Room room) {
            return string.Empty;
        }

        private static bool IsSuccess(string response) {
            return true;
        }

        private static IpString GetIpFromResponse(string response) {
            return new IpString(MASTER_IP);
        }

        private static string GetErrorFromResponse(string error) {
            return "Some error";
        }
    }
}