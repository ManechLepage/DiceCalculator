#include <iostream>
#include <chrono>
#include "io.hpp"
#include <filesystem>

int main() {
    pcg32_fast_init(23738);
    IOManager io("commands.txt", "results.txt");
    while (true) {
        std::string command = io.blockUntilUpdate();
        io.writeResult(command);
    }

}