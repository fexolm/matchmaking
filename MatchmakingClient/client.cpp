//
// Created by fexolm on 7/22/17.
//

#include "client.h"

client::client() :
        ios_(),
        socket_(ios_),
        handlers_() {}

void client::connect(const std::string &ip, int port) {
    tcp::endpoint endpoint(boost::asio::ip::address::from_string(ip), port);
    socket_.connect(endpoint);
}

void client::send(const ptree_t &pt) {
    std::ostringstream oss;
    boost::property_tree::json_parser::write_json(oss, pt);
    std::string to_send = oss.str();
    boost::array<char, 512> buffer;
    std::copy(to_send.begin(), to_send.end(), buffer.begin());
    boost::system::error_code error;
    boost::asio::write(socket_, boost::asio::buffer(buffer, to_send.length()), error);
}


std::string read_one_json(std::string &str) {
    const static std::string EMPTY_STR = "";
    int brackets_count = 0;
    int i = 0;
    do {
        if (str[i] == '{') brackets_count++;
        if (str[i] == '}') brackets_count--;
        i++;
    } while (brackets_count > 0);

    std::string result = str.substr(0, i);
    if (i == str.length()) {
        str = EMPTY_STR;
    } else {
        str = str.substr(i, str.length());
    }
    if (str.find("{") == std::string::npos) {
        str = EMPTY_STR;
    }

    return result;
}

void client::tick() {
    if (socket_.available()) {
        boost::array<char, 1024> buf;
        socket_.read_some(boost::asio::buffer(buf));
        std::string msgFull;
        std::copy(buf.begin(), buf.end(), std::back_inserter(msgFull));
        std::string msg = read_one_json(msgFull);
        std::istringstream st(msg);
        boost::property_tree::ptree pt;
        boost::property_tree::read_json(st, pt);
        int id = pt.get<int>("Id");
        handlers_[id](id, pt);
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