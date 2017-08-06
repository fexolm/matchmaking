using System.Collections.Generic;

namespace HistoryBattlesServer.Rooms
{
    //TODO: implement
    public class RoomParams
    {
        public string Nickname { get; set; }
        public int Bet { get; set; }
        public string Map { get; set; }
        public string Fraction { get; set; }
        public int Rang { get; set; }

        public void Deserialize(List<string> parameters) {
            Nickname = parameters[0];
            Bet = int.Parse(parameters[1]);
            Map = parameters[2];
            Fraction = parameters[3];
            Rang = int.Parse(parameters[4]);
        }
    }
}