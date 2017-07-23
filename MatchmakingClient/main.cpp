#include <iostream>
#include <boost/thread.hpp>
#include "matchmaking_client.h"
#include "responses.h"

int main() {
    std::string token;
    std::cin >> token;
    matchmaking_client c(token);
    c.connect("127.0.0.1", 8001);
    bool end = false;
    std::string command;

    c.set_on_game_started([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        ip_response resp(params);
        if (resp.success()) {
            std::cout << "game started at " << resp.get_ip() << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_room_closed([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        response resp(params);
        if (resp.success()) {
            std::cout << "room closed" << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_opponent_leaved([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        response resp(params);
        if (resp.success()) {
            std::cout << "opponent leaved " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_leave_room([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        response resp(params);
        if (resp.success()) {
            std::cout << "successfully leaved " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_join_room([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        response resp(params);
        if (resp.success()) {
            std::cout << "successfully joined " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_create_room([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        response resp(params);
        if (resp.success()) {
            std::cout << "successfully created " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_show_rooms([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        room_list_response resp(params);
        if (resp.success()) {
            auto rooms = resp.get_rooms();
            for (auto iter = rooms.begin(); iter != rooms.end(); ++iter) {
                std::cout << iter->get_token() << " " << iter->get_map() << std::endl;
            }
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_player_joined([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        token_response resp(params);
        if (resp.success()) {
            std::cout << "player joined " << resp.get_token() << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });

    std::map<std::string, std::function<void(void)>> handlers;
    handlers["exit"] = [&end] { end = false; };
    handlers["start"] = [&c]() { c.start_game(); };
    handlers["create"] = [&c]() {
        std::string nickname, map, fraction, rang;
        int bet;
        std::cout << "nickname: ";
        std::cin >> nickname;
        std::cout << "map: ";
        std::cin >> map;
        std::cout << "fraction: ";
        std::cin >> fraction;
        std::cout << "rang: ";
        std::cin >> rang;
        std::cout << "bet: ";
        std::cin >> bet;
        create_room_request request(nickname, bet, map, fraction, rang);
        c.create_room(request);
    };
    handlers["join"] = [&c]() {
        std::string room_token;
        std::cout << "room token: ";
        std::cin >> room_token;
        c.join_room(room_token);
    };
    handlers["leave"] = [&c]() { c.leave_room(); };
    handlers["show"] = [&c]() { c.show_rooms(); };

    boost::thread thread([&c, &end]() { while (!end) { c.tick(); }});

    while (!end) {
        std::cout << "command: ";
        std::cin >> command;
        handlers[command]();
    }
}