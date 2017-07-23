//
// Created by fexolm on 7/22/17.
//

#ifndef MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
#define MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H


#include <string>
#include <functional>
#include "client.h"
#include "room.h"


class matchmaking_client : public client {
private:
    std::string token_;
public:
    matchmaking_client(const std::string &token);

    void show_rooms();

    void start_game();

    void create_room(const create_room_request&);

    void join_room(const std::string &);

    void leave_room();

    void set_on_create_room(client::handler_t);

    void set_on_show_rooms(client::handler_t);

    void set_on_join_room(client::handler_t);

    void set_on_leave_room(client::handler_t);

    void set_on_game_started(client::handler_t);

    void set_on_opponent_leaved(client::handler_t);

    void set_on_room_closed(client::handler_t);

    void set_on_player_joined(client::handler_t);
};


#endif //MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
