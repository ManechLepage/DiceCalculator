#include <iostream>
#include <chrono>
#include "io.hpp"

int main() {
    pcg32_fast_init(23738);
    IOManager io("commands.txt", "results.txt");
    while (true) {
        std::string command = io.blockUntilUpdate();
        std::cout << "Received command: " << command << std::endl;
        io.writeResult(command);
    }

}