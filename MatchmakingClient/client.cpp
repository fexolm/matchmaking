//
// Created by fexolm on 7/22/17.
//

#include "client.h"
#include <boost/algorithm/string.hpp>

client::client() :
        ios_(),
        socket_(ios_),
        handlers_() {}

void client::connect(const std::string &ip, int port) {
    tcp::endpoint endpoint(boost::asio::ip::address::from_string(ip), port);
    socket_.connect(endpoint);
}

void client::send(const message &msg) {
    auto to_send = msg.get_value();
    boost::array<char, 512> buffer;
    std::copy(to_send.begin(), to_send.end(), buffer.begin());
    boost::system::error_code error;
    boost::asio::write(socket_, boost::asio::buffer(buffer, to_send.length()), error);
}

void client::tick() {
    if (socket_.available()) {
        boost::asio::streambuf buffer;
        boost::system::error_code error;
        size_t len = boost::asio::read_until(socket_, buffer, '|', error);
        std::stringstream message_stream;
        message_stream.write(boost::asio::buffer_cast<const char *>(buffer.data()), len);
        auto msg = message_stream.str();
        msg.pop_back();
        client::params_t full_params;
        boost::algorithm::split(full_params, msg, boost::algorithm::is_any_of(" "));
        int id = std::stoi(full_params[0]);
        std::string token = full_params[1];
        client::params_t params(full_params.size() - 2);
        std::copy(full_params.begin()+2, full_params.end(), params.begin());
        handlers_[id](id, token, params);
    }
}

client::~client() {
    disconnect();
}

void client::disconnect() {
    if (socket_.is_open()) {
        socket_.close();
    }
}

void client::add_handler(int id, client::handler_t handler) {
    handlers_[id] = handler;
}
