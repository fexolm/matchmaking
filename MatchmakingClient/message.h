//
// Created by fexolm on 7/23/17.
//

#ifndef MATCHMAKINGCLIENT_MESSAGE_H
#define MATCHMAKINGCLIENT_MESSAGE_H
#include <string>
#include <sstream>


class message {
private:
    std::string msg_string_;
public:
    explicit message(int id, std::string token, std::string msg_params = "") {
        std::stringstream fmt;
        fmt << id << " " << token << " " << msg_params << '|';
        msg_string_ = fmt.str();
    }
    std::string get_value() const {
        return msg_string_;
    }
};

#endif //MATCHMAKINGCLIENT_MESSAGE_H

