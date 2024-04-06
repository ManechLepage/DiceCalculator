#include "io.hpp"

std::string IOManager::blockUntilUpdate() {
    int new_id;
    std::string command;
    while (true) {
        this->command_file.seekg(0);
        this->command_file >> new_id;
        if (new_id != id) {
            id = new_id + 1;
            std::getline(this->command_file, command);
            return command;
        }
    }
}

void IOManager::writeResult(std::string command) {
    OpTree* tree = constructOpTree(command);
    Dist dist = tree->getDist(SIMS);
    for (int i = 0; i < dist.prob.size(); i++) {
        this->result_file << "(" << i + dist.range.first << ", " << dist.prob[i] << ")";
    }
    delete tree;
}