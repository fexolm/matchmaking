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
    room(const std::string &nickname,
         const std::string &map,
         const std::string &bet,
         const std::string &fraction,
         const std::string &rang) :
            nickname_(nickname),
            map_(map),
            bet_(bet),
            fraction_(fraction),
            rang_(rang) {}

    room(const boost::property_tree::ptree &pt) :
            token_(pt.get<std::string>("Room.Token")),
            nickname_(pt.get<std::string>("Room.Nockname")),
            map_(pt.get<std::string>("Room.Map")),
            bet_(pt.get<std::string>("Room.Bet")),
            fraction_(pt.get<std::string>("Room.Fraction")),
            rang_(pt.get<std::string>("Room.Rang")) {
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

    void serialize_to(boost::property_tree::ptree &pt) const {
        pt.put("Room.Nickname", nickname_);
        pt.put("Room.Map", map_);
        pt.put("Room.Bet", bet_);
        pt.put("Room.Fraction", fraction_);
        pt.put("Room.Rang", rang_);
    }
};

#endif //MATCHMAKINGCLIENT_ROOM_H
