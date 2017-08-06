#include <iostream>
#include <boost/thread.hpp>
#include "matchmaking_client.h"

int main() {
    std::string token;
    std::cin >> token;
    matchmaking_client c(token);
    c.connect("192.168.0.102", 8001);
    bool end = false;
    std::string command;

    c.set_on_game_started([&c](const ip_response &resp) {
        if (resp.success()) {
            std::cout << "game started at " << resp.get_ip() << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_room_closed([&c](const response &resp) {
        if (resp.success()) {
            std::cout << "room closed" << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_opponent_leaved([&c](const response &resp) {
        if (resp.success()) {
            std::cout << "opponent leaved " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_leave_room([&c](const response &resp) {
        if (resp.success()) {
            std::cout << "successfully leaved " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_join_room([&c](const response &resp) {
        if (resp.success()) {
            std::cout << "successfully joined " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_create_room([&c](const response &resp) {
        if (resp.success()) {
            std::cout << "successfully created " << std::endl;
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_show_rooms([&c](const room_list_response &resp) {
        if (resp.success()) {
            auto rooms = resp.get_rooms();
            for (auto iter = rooms.begin(); iter != rooms.end(); ++iter) {
                std::cout << iter->get_token() << " " << iter->get_map() << std::endl;
            }
        } else {
            std::cout << resp.get_error() << std::endl;
        }
    });
    c.set_on_player_joined([&c](const token_response &resp) {
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
        std::string nickname, map, fraction, rang, bet;
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
        room r(nickname, map, bet, fraction, rang);
        c.create_room(r);
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