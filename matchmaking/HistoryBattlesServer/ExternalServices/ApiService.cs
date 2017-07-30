using System.Threading.Tasks;
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

    public class IpResponse
    {
        public string Ip { get; set; }

        public bool Success { get; set; }
    }

    public static class ApiService
    {
        //TODO: change master ip
        private const string MASTER_IP = "192.168.0.1";

        //TODO: implement
        public static Result ValidateRoomParams(RoomParams roomParams, Player player) {
            return Result.Ok;
        }

        //TODO: test
        public static async Task<ValueResult<IpString>> GetRemoteServerIp(Room room) {
            var response = await WebService.Post<Room, IpResponse>(MASTER_IP, room);
            return new ValueResult<IpString>(new IpString(response.Ip));
        }
    }
}