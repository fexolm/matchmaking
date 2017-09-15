//
// Created by fexolm on 8/5/17.
//

#ifndef MATCHMAKINGCLIENT_REQUEST_H
#define MATCHMAKINGCLIENT_REQUEST_H

#include "message.h"

class join_room_request : message {
private:
    std::string token_;
public:
    join_room_request(int id, const std::string &token)
            : message(id),
              token_(token) {}

    std::string get_token() {
        return token_;
    }

    virtual boost::property_tree::ptree serialize() override {
        auto pt = message::serialize();
        pt.put("RoomToken", token_);
        return pt;
    }
};

#endif //MATCHMAKINGCLIENT_REQUEST_H
