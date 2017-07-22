//
// Created by fexolm on 7/22/17.
//

#ifndef MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
#define MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H


#include <string>
#include <functional>
#include "client.h"


class matchmaking_client : private client {
private:
    std::string token_;
public:
    matchmaking_client(std::string token);
    void show_rooms();
    void create_room();
    void join_room();
    void leave_room();
};


#endif //MATCHMAKINGCLIENT_MATCHMAKING_CLIENT_H
