//
// Created by fexolm on 7/22/17.
//

#include "message_types.h"
#include "matchmaking_client.h"

matchmaking_client::matchmaking_client(const std::string &token) :
        token_(token) {
}

void matchmaking_client::show_rooms() {
    client::send(message(SHOW_ROOMS, token_));
}

void matchmaking_client::create_room(const create_room_request &request) {
    client::send(message(CREATE_ROOM, token_, request.str()));
}

void matchmaking_client::join_room(const std::string &room_token) {
    client::send(message(JOIN_ROOM, token_, room_token));
}


void matchmaking_client::leave_room() {
    client::send(message(LEAVE_ROOM, token_));
}

void matchmaking_client::start_game() {
    client::send(message(START_GAME, token_));
}

void matchmaking_client::set_on_create_room(matchmaking_client::response_handler_t handler) {
    client::add_handler(CREATE_ROOM, [handler](int id, const std::string &t, client::params_t params) {
        handler(response(params));
    });
}

void matchmaking_client::set_on_show_rooms(matchmaking_client::room_list__response_handler_t handler) {
    client::add_handler(SHOW_ROOMS, [handler](int id, const std::string &t, client::params_t params) {
        handler(room_list_response(params));
    });
}

void matchmaking_client::set_on_join_room(matchmaking_client::response_handler_t handler) {
    client::add_handler(JOIN_ROOM, [handler](int id, const std::string &t, client::params_t params) {
        handler(response(params));
    });
}

void matchmaking_client::set_on_leave_room(matchmaking_client::response_handler_t handler) {
    client::add_handler(LEAVE_ROOM, [handler](int id, const std::string &t, client::params_t params) {
        handler(response(params));
    });
}

void matchmaking_client::set_on_game_started(matchmaking_client::ip_response_handler_t handler) {
    client::add_handler(START_GAME, [handler](int id, const std::string &t, client::params_t params) {
        handler(ip_response(params));
    });
}

void matchmaking_client::set_on_opponent_leaved(matchmaking_client::token_response_handler_t handler) {
    client::add_handler(OPPONENT_LEAVED, [handler](int id, const std::string &t, client::params_t params) {
        handler(token_response(params));
    });
}

void matchmaking_client::set_on_room_closed(matchmaking_client::response_handler_t handler) {
    client::add_handler(ROOM_CLOSED, [handler](int id, const std::string &t, client::params_t params) {
        handler(response(params));
    });
}

void matchmaking_client::set_on_player_joined(matchmaking_client::token_response_handler_t handler) {
    client::add_handler(PLAYER_JOINED, [handler](int id, const std::string &t, client::params_t params) {
        handler(token_response(params));
    });
}