//
// Created by fexolm on 7/22/17.
//

#include "message_types.h"
#include "matchmaking_client.h"

matchmaking_client::matchmaking_client(const std::string &token) :
        token_(token) {
    client::add_handler(SHOW_ROOMS, on_show_rooms);
    client::add_handler(CREATE_ROOM, on_create_room);
    client::add_handler(JOIN_ROOM, on_join_room);
    client::add_handler(LEAVE_ROOM, on_leave_room);
    client::add_handler(START_GAME, on_game_started);
    client::add_handler(OPPONENT_LEAVED, on_opponent_leaved);
    client::add_handler(ROOM_CLOSED, on_room_closed);
    client::add_handler(PLAYER_JOINED, on_player_joined);
}

void matchmaking_client::show_rooms() {
    client::send(message(SHOW_ROOMS, token_));
}

void matchmaking_client::create_room() {
    client::send(message(CREATE_ROOM, token_));
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
