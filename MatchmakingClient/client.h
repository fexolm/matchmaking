//
// Created by fexolm on 7/22/17.
//
#ifndef MATCHMAKINGCLIENT_CLIENT_H
#define MATCHMAKINGCLIENT_CLIENT_H

#include <map>
#include <string>
#include <vector>
#include <functional>
#include <boost/asio.hpp>
#include <boost/array.hpp>
#include <boost/algorithm/string.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>
#include "message.h"

using boost::asio::ip::tcp;

class client {
public:
    typedef boost::property_tree::ptree ptree_t;
    typedef std::vector<std::string> params_t;
    typedef std::function<void(int id, const ptree_t &)> handler_t;

private:
    boost::asio::io_service ios_;
    tcp::socket socket_;
    std::map<int, handler_t> handlers_;
public:

    client();

    virtual void connect(const std::string &ip, int port);

    virtual void send(const ptree_t &);

    virtual void tick();

    virtual ~client();

    virtual void disconnect();

    virtual void add_handler(int id, handler_t handler);
};


#endif //MATCHMAKINGCLIENT_CLIENT_H
