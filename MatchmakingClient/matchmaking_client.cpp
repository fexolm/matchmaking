//
// Created by fexolm on 7/22/17.
//

#include "message_types.h"
#include "matchmaking_client.h"
#include "request.h"

matchmaking_client::matchmaking_client(const std::string &token) :
        token_(token) {
}

void matchmaking_client::show_rooms() {
    send(message(SHOW_ROOMS).serialize());
}

void matchmaking_client::create_room(const room &room) {
    auto msg = message(CREATE_ROOM).serialize();
    room.serialize_to(msg);
    send(msg);
}

void matchmaking_client::join_room(const std::string &room_token) {
    send(join_room_request(JOIN_ROOM, room_token).serialize());
}


void matchmaking_client::leave_room() {
    send(message(LEAVE_ROOM).serialize());
}

void matchmaking_client::start_game() {
    send(message(START_GAME).serialize());
}

void matchmaking_client::set_on_create_room(matchmaking_client::response_handler_t handler) {
    add_handler(CREATE_ROOM, [handler](int id, const ptree_t &pt) {
        response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_show_rooms(matchmaking_client::room_list__response_handler_t handler) {
    add_handler(SHOW_ROOMS, [handler](int id, const ptree_t &pt) {
        room_list_response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_join_room(matchmaking_client::response_handler_t handler) {
    add_handler(JOIN_ROOM, [handler](int id, const ptree_t &pt) {
        response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_leave_room(matchmaking_client::response_handler_t handler) {
    add_handler(LEAVE_ROOM, [handler](int id, const ptree_t &pt) {
        response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_game_started(matchmaking_client::ip_response_handler_t handler) {
    add_handler(START_GAME, [handler](int id, const ptree_t &pt) {
        ip_response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_opponent_leaved(matchmaking_client::response_handler_t handler) {
    add_handler(OPPONENT_LEAVED, [handler](int id, const ptree_t &pt) {
        response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_room_closed(matchmaking_client::response_handler_t handler) {
    add_handler(ROOM_CLOSED, [handler](int id, const ptree_t &pt) {
        response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::set_on_player_joined(matchmaking_client::token_response_handler_t handler) {
    add_handler(PLAYER_JOINED, [handler](int id, const ptree_t &pt) {
        token_response resp;
        resp.deserialize(pt);
        handler(resp);
    });
}

void matchmaking_client::send(const boost::property_tree::ptree &pt) {
    boost::property_tree::ptree pt_copy(pt);
    pt_copy.put("Player.Token", token_);
    client::send(pt_copy);
}

