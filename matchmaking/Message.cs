namespace matchmaking
{
    public class Message<TPlayer> : ISerializable
        where TPlayer : Player
    {
        public readonly int Id;
        public readonly TPlayer Player;

        public Message(int id, TPlayer player) {
            Id = id;
            Player = player;
        }

        public virtual string Serialize() {
            return $"{Id} {Player.Token}";
        }
    }
}