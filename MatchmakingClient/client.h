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

using boost::asio::ip::tcp;

class client {
public:
    typedef std::vector<std::string> params_t;
    typedef std::function<void(int id, const std::string &token, params_t params)> handler_t;

private:
    boost::asio::io_service ios_;
    tcp::socket socket_;
    std::map<int, handler_t> handlers_;
public:

    client();

    void connect(const std::string &ip, int port);

    void send(const std::string &msg);

    void tick();

    virtual ~client();

    void disconnect();

    void add_handler(int id, handler_t handler);
};


#endif //MATCHMAKINGCLIENT_CLIENT_H
