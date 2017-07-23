//
// Created by fexolm on 7/22/17.
//

#ifndef MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
#define MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H


#include <string>
#include <functional>
#include "client.h"


class matchmaking_client : public client {
private:
    std::string token_;
public:
    matchmaking_client(const std::string &token);
    void show_rooms();
    void start_game();
    void create_room();
    void join_room(const std::string &);
    void leave_room();
    client::handler_t on_create_room;
    client::handler_t on_show_rooms;
    client::handler_t on_join_room;
    client::handler_t on_leave_room;
    client::handler_t on_game_started;
    client::handler_t on_opponent_leaved;
    client::handler_t on_room_closed;
    client::handler_t on_player_joined;
};


#endif //MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
