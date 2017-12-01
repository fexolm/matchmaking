using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HistoryBattlesServer.Requests
{
    public class GetUserRequest
    {
        public readonly string Token;

        public GetUserRequest(string token) {
            Token = token;
        }
    }
}