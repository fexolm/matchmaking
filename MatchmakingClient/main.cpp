#include <iostream>
#include "client.h"

int main() {
    client c;
    c.connect("127.0.0.1", 8001);
    c.send("1 KEK 123 hello|");
    bool end = false;
    c.add_handler(1, [&end](int id, std::string token, client::params_t params){
        std::cout << token << params[0] << std::endl;
        end = true;
    });
    while (!end) {
        c.tick();
    }
}