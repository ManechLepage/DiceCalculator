#include <fstream>
#include <ostream>
#include <string>
#include "dicecalc.hpp"
constexpr int SIMS = 100000;

class IOManager {
public:
    IOManager(std::string command_name, std::string result_name) {
        this->command_file.open(command_name, std::ios::in);
        this->result_name = result_name;
        rid = -1;
        wid = -1;
        
    }

    ~IOManager() {
        this->command_file.close();
    }

    std::string blockUntilUpdate();
    void writeResult(std::string command);

private:
    int rid;
    int wid;
    std::ifstream command_file;
    std::string result_name;
    std::ofstream result_file;
};