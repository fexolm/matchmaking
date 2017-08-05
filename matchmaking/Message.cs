namespace matchmaking
{
    public class Message<TPlayer>
        where TPlayer : Player
    {
        public readonly int Id;
        public readonly TPlayer Player;

        public Message(int id, TPlayer player) {
            Id = id;
            Player = player;
        }
    }
}