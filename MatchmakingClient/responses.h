//
// Created by fexolm on 7/23/17.
//

#ifndef MATCHMAKINGCLIENT_RESPONSES_H
#define MATCHMAKINGCLIENT_RESPONSES_H

#include <map>
#include <string>
#include <sstream>
#include <boost/algorithm/string.hpp>
#include <boost/foreach.hpp>
#include <iostream>
#include "client.h"
#include "room.h"

class response : public message {
private:
    bool success_;
    std::string error_message_;
public:

    bool success() const {
        return success_;
    }

    std::string get_error() const {
        return error_message_;
    }

    virtual void deserialize(const boost::property_tree::ptree &pt) override {
        message::deserialize(pt);
        success_ = pt.get<bool>("Result.Success");
        error_message_ = pt.get<std::string>("Result.ErrorMessage");
    }
};


class ip_response : public response {
private:
    std::string ip_;
public:
    std::string get_ip() const {
        return ip_;
    }

    virtual void deserialize(const boost::property_tree::ptree &pt) override {
        response::deserialize(pt);
        if (success()) {
            ip_ = pt.get<std::string>("Result.Value");
        }
    }
};

class token_response : public response {
private:
    std::string token_;
public:
    std::string get_token() const {
        return token_;
    }

    virtual void deserialize(const boost::property_tree::ptree &pt) override {
        response::deserialize(pt);
        token_ = pt.get<std::string>("Result.Value");
    }
};

class room_list_response : public response {
private:
    std::vector<room> rooms_;
public:
    std::vector<room> get_rooms() const {
        return rooms_;
    }

    virtual void deserialize(const boost::property_tree::ptree &pt) override {
        response::deserialize(pt);
        int count = pt.get<int>("Count");
        if(count == 0) return;
        //@formatter:off
        BOOST_FOREACH(const boost::property_tree::ptree::value_type &tree_node_value, pt.get_child("Result.Value")) {
            const boost::property_tree::ptree subtree = (boost::property_tree::ptree) tree_node_value.second;
            rooms_.push_back(room(subtree));
        }
        //@formatter:on
    }
};


#endif //MATCHMAKINGCLIENT_RESPONSES_H
