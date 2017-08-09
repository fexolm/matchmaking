using matchmaking;

namespace FootballClient.Models.Results
{
    class Result : Message<Player>
    {
        public Result(int id, Player player) : base(id, player)
        {
        }
    }
}
