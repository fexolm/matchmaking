using FootballServer.Enums;
using matchmaking;

namespace FootballServer.Models.Results
{
    class ErrorResult : Result
    {
        public readonly string ErrorMessage;

        public ErrorResult(string errorMessage, Player player) : base((int)MessageType.ERROR, player)
        {
            ErrorMessage = errorMessage;
        }
    }
}