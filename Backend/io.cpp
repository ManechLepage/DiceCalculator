#include "io.hpp"

std::string IOManager::blockUntilUpdate() {
    int new_id;
    std::string command;
    while (true) {
        this->command_file.seekg(0);
        if (this->command_file.peek() == std::ifstream::traits_type::eof()) {
            continue;
        }
        this->command_file >> new_id;
        if (new_id != rid) {
            rid = new_id;
            wid = new_id + 1;
            this->command_file.ignore(2, '\n');
            std::getline(this->command_file, command);
            return command;
        }
    }
}

void IOManager::writeResult(std::string command) {
    this->result_file.open(this->result_name, std::ios::out | std::ios::trunc);
    OpTree* tree = constructOpTree(command);
    Dist dist = tree->getDist(SIMS);
    this->result_file << wid << std::endl;
    for (unsigned int i = 0; i < dist.prob.size(); i++) {
        this->result_file << i + dist.range.first << " " << 100 * (double)dist.prob[i]/dist.total << std::endl;
    }
    this->result_file.close();
    delete tree;
}