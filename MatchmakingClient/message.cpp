//
// Created by fexolm on 8/4/17.
//

#include "message.h"


int message::get_id() const {
    return id_;
}

void message::deserialize(const boost::property_tree::ptree &pt) {
    id_ = pt.get<int>("Id");
}


boost::property_tree::ptree message::serialize() {
    boost::property_tree::ptree pt;
    pt.put("Id", id_);
}

message::~message() {

}

message::message() {

}

message::message(int id) {
    id_ = id;
}
