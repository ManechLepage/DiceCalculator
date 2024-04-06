#include <iostream>
#include "search.hpp"

int main() {
    pcg32_fast_init(23738);
    /* IOManager io("command.txt", "result.txt");
    while (true) {
        std::string command = io.blockUntilUpdate();
        io.writeResult(command);
    } */
    Dist target = constructOpTree("[% 2 d6]")->getDist(1000);
    Genome best = anneal(target, 7);
    return 0;    
}