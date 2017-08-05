//
// Created by fexolm on 7/23/17.
//

#ifndef MATCHMAKINGCLIENT_MESSAGE_H
#define MATCHMAKINGCLIENT_MESSAGE_H

#include <string>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>

class message {
private:
    int id_;
public:
    message();

    message(int id);

    int get_id() const;

    virtual void deserialize(const boost::property_tree::ptree &);

    virtual boost::property_tree::ptree serialize();

    virtual ~message();
};

#endif //MATCHMAKINGCLIENT_MESSAGE_H

