﻿namespace HistoryBattlesServer.Rooms
{
    public class Room
    {
        public readonly HBPlayer Owner;
        public HBPlayer Other;
        public readonly RoomParams Params;

        public Room(HBPlayer player, RoomParams roomParams) {
            Owner = player;
            Params = roomParams;
        }

        public void Join(HBPlayer player) {
            Other = player;
        }

        public string Serialize() {
            return string.Join("-",
                Owner.Token,
                Params.Nickname,
                Params.Map,
                Params.Bet,
                Params.Fraction,
                Params.Rang);
        }
    }
}