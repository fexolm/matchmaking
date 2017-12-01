//
// Created by fexolm on 7/22/17.
//

#ifndef MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
#define MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H


#include <string>
#include <functional>
#include "client.h"
#include "room.h"
#include "responses.h"


class matchmaking_client : public client {
public:
    typedef std::function<void(const response &)> response_handler_t;
    typedef std::function<void(const ip_response &)> ip_response_handler_t;
    typedef std::function<void(const token_response &)> token_response_handler_t;
    typedef std::function<void(const room_list_response &)> room_list__response_handler_t;

private:
    std::string token_;

public:

    matchmaking_client(const std::string &token);

    void show_rooms();

    void start_game();

    void create_room(const room &);

    void join_room(const std::string &);

    void leave_room();

    void set_on_create_room(response_handler_t);

    void set_on_show_rooms(room_list__response_handler_t);

    void set_on_join_room(response_handler_t);

    void set_on_leave_room(response_handler_t);

    void set_on_game_started(ip_response_handler_t);

    void set_on_opponent_leaved(response_handler_t);

    void set_on_room_closed(response_handler_t);

    void set_on_player_joined(token_response_handler_t);

    virtual void send(const boost::property_tree::ptree &pt) override;
};


#endif //MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
