#include <iostream>
#include "matchmaking_client.h"

int main() {
    std::string token;
    std::cin >> token;
    matchmaking_client c(token);
    c.connect("127.0.0.1", 8001);
    bool end = false;
    std::string command;

    c.set_on_game_started([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "game started -- " << params[0];
    });
    c.set_on_room_closed([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "room was closed -- " << params[0];
    });
    c.set_on_opponent_leaved([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "opponent leaved -- " << params[0];
    });
    c.set_on_leave_room([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "successfully leaved -- " << params[0];
    });
    c.set_on_join_room([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "room was closed -- " << params[0];
    });
    c.set_on_create_room([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "room created -- " << params[0];
    });
    c.set_on_show_rooms([&c](int id, const std::string &t, matchmaking_client::params_t params) {

    });
    c.set_on_player_joined([&c](int id, const std::string &t, matchmaking_client::params_t params) {
        std::cout << "player joined -- " << params[0];
    });

    std::map<std::string, std::function<void(void)>> handlers;
    handlers["exit"] = [&end] { end = false; };
    handlers["start"] = [&c]() { c.start_game(); };
    handlers["create"] = [&c]() { c.create_room(); };
    handlers["join"] = [&c]() {
        std::string room_token;
        std::cout << "room token: ";
        std::cin >> room_token;
        c.join_room(room_token);
    };
    handlers["leave"] = [&c]() { c.leave_room(); };
    handlers["show"] = [&c]() { c.show_rooms(); };


    while (!end) {
        std::cout << "command: ";
        std::cin >> command;
        handlers[command]();
        c.tick();
    }
}