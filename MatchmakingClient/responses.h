//
// Created by fexolm on 7/23/17.
//

#ifndef MATCHMAKINGCLIENT_RESPONSES_H
#define MATCHMAKINGCLIENT_RESPONSES_H

#include <map>
#include <string>
#include <sstream>
#include <boost/algorithm/string.hpp>
#include "client.h"
#include "room.h"

class response {
private:
    client::params_t params;
    std::string status_;
    std::string error_message_;
public:
    response(const client::params_t &params) :
            params(params),
            status_(params[0]) {
        if (status_ == "ERROR") {
            std::stringstream sstream;
            for (auto iter = params.begin() + 1; iter != params.end(); ++iter) {
                sstream << *iter << " ";
            }
            error_message_ = sstream.str();
        }
    }

    bool success() const {
        return status_ == "OK";
    }

    std::string get_error() const {
        return error_message_;
    }

};


class ip_response : public response {
private:
    std::string ip_;
public:
    ip_response(const client::params_t &params) : response(params) {
        if (success()) {
            ip_ = std::stoi(params[1]);
        }
    }

    std::string get_ip() const {
        return ip_;
    }
};

class token_response : public response {
private:
    std::string token_;
public:
    token_response(const client::params_t &params) : response(params) {
        if (success()) {
            token_ = params[1];
        }
    }

    std::string get_token() const {
        return token_;
    }
};

class room_list_response : public response {
private:
    std::vector<room> rooms_;
public:
    room_list_response(const client::params_t &params) : response(params) {
        if (success()) {
            for (auto iter = params.begin() + 1; iter != params.end(); ++iter) {
                client::params_t res;
                boost::algorithm::split(res, *iter, boost::algorithm::is_any_of("-"));
                room r(res);
                rooms_.push_back(r);
            }
        }
    }

    std::vector<room> get_rooms() const {
        return rooms_;
    }
};


#endif //MATCHMAKINGCLIENT_RESPONSES_H
