//
// Created by fexolm on 7/23/17.
//

#ifndef MATCHMAKINGCLIENT_ROOM_H
#define MATCHMAKINGCLIENT_ROOM_H

#include <string>
#include <sstream>
#include "client.h"

class room {
private:
    std::string token_;
    std::string nickname_;
    std::string map_;
    std::string bet_;
    std::string fraction_;
    std::string rang_;
public:
    room(const client::params_t &room_params) :
            token_(room_params[0]),
            nickname_(room_params[1]),
            map_(room_params[2]),
            bet_(room_params[3]),
            fraction_(room_params[4]),
            rang_(room_params[5]) {
    }

    std::string get_token() const {
        return token_;
    }

    std::string get_nickname() const {
        return nickname_;
    }

    std::string get_map() const {
        return map_;
    }

    std::string get_bet() const {
        return bet_;
    }

    std::string get_fraction() const {
        return fraction_;
    }

    std::string get_rang() const {
        return rang_;
    }
};

class create_room_request {
private:
    std::string request_str;
public:
    create_room_request(
            std::string nickname,
            int bet,
            std::string map,
            std::string fraction,
            std::string rang) {
        std::stringstream sstream;
        sstream << nickname << " " << bet << " " << map << " " << fraction << " " << rang;
        request_str = sstream.str();
    }

    std::string str() const {
        return request_str;
    }
};

#endif //MATCHMAKINGCLIENT_ROOM_H
