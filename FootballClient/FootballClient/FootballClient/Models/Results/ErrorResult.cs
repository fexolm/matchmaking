using FootballClient.Enums;

namespace FootballClient.Models.Results
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