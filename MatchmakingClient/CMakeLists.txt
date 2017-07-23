cmake_minimum_required(VERSION 3.8)
project(MatchmakingClient)

set(CMAKE_CXX_STANDARD 17)

set(SOURCE_FILES main.cpp client.cpp client.h matchmaking_client.cpp matchmaking_client.h message_types.h message.h)

find_package(Boost COMPONENTS system thread REQUIRED)


add_executable(MatchmakingClient ${SOURCE_FILES})

INCLUDE_DIRECTORIES(${Boost_INCLUDE_DIR})

TARGET_LINK_LIBRARIES(MatchmakingClient LINK_PUBLIC ${Boost_LIBRARIES})