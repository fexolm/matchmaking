namespace HistoryBattlesServer
{
    public enum MessageType : int
    {
        CREATE_ROOM,
        JOIN_ROOM,
        LEAVE_ROOM,
        START_GAME,
        SHOW_ROOMS,
        OPPONENT_LEAVED,
        ROOM_CLOSED,
        PLAYER_JOINED,
    }
}