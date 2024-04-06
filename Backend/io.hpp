#include <fstream>
#include <ostream>
#include <string>
#include "dicecalc.hpp"
constexpr int SIMS = 100000;

class IOManager {
public:
    IOManager(std::string command_name, std::string result_name) {
        this->command_file.open(command_name, std::ios::out);
        this->result_file.open(result_name, std::ios::in);
        id = -1;
        
    }

    ~IOManager() {
        this->command_file.close();
        this->result_file.close();
    }

    std::string blockUntilUpdate();
    void writeResult(std::string command);

private:
    int id;
    std::ifstream command_file;
    std::ofstream result_file;
};