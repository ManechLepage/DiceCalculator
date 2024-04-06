#include <vector>
#include <algorithm>
#include <utility>
#include <cmath>
#include <iostream>
#include <string>
#include <map>
#include "random.hpp"

enum Op {
    SCALAR,
    DIE,
    ROLL,
    PLUS,
    MINUS,
    MULTIPLY,
    DIVIDE,
    ADVANTAGE,
    DISADVANTAGE
};

typedef std::pair<int, int> Range;



struct Dist {
    Range range;
    int total;
    int getSize() {
        return range.second - range.first + 1;
    }
    std::vector<long int> prob;
};

double getErr(const Dist& target, const Dist& given);


class OpTree {
public:
    OpTree(Op op, OpTree* left, OpTree* right) {
        this->op = op;
        this->left = left;
        this->right = right;
        this->val = 0;
    }
    OpTree(int val, Op op) {
        this->op = op;
        this->val = val;
        this->left = nullptr;
        this->right = nullptr;
    }

    ~OpTree() {
        if (this->left != nullptr) {
            delete left;
        }
        if (this->right != nullptr) {
            delete right;
        }
    }

    std::string repr();
    int simul();
    Range getRange();
    Dist getDist(int num_sims);

private:
    Op op;
    OpTree* left;
    OpTree* right;
    int val; //number if SCALAR, number of sides if DIE, else 0
};


OpTree* constructOpTree(std::string command);
