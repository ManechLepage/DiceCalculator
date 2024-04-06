#include "dicecalc.hpp"

std::string OpTree::repr() {
    std::string result = "";
    switch (this->op) {
        case SCALAR:
            result += std::to_string(this->val);
            break;
        case DIE:
            result += "d" + std::to_string(this->val);
            break;
        case ROLL:
            result += "[% " + this->left->repr() + " " + this->right->repr() + "]";
            break;
        case PLUS:
            result += "[+ " + this->left->repr() + " " + this->right->repr() + "]";
            break;
        case MINUS:
            result += "[- " + this->left->repr() + " " + this->right->repr() + "]";
            break;
        case MULTIPLY:
            result += "[* " + this->left->repr() + " " + this->right->repr() + "]";
            break;
        case DIVIDE:
            result += "[/ " + this->left->repr() + " " + this->right->repr() + "]";
            break;
        case ADVANTAGE:
            result += "[adv " + this->left->repr() + " " + this->right->repr() + "]";
            break;
        case DISADVANTAGE:
            result += "[dis " + this->left->repr() + " " + this->right->repr() + "]";
            break;
    }
    return result;
}

int OpTree::simul() {
    Dist dist;
    switch (this->op) {
        case SCALAR:
            return this->val;
        case DIE:
            return random(this->val) + 1;
        case ROLL:
            {
                int total = 0;
                int num_rolls = this->left->simul();
                for (int i = 0; i < num_rolls; i++) {
                    total += this->right->simul();
                }
                return total;
            }
        case PLUS:
            return this->left->simul() + this->right->simul();
        case MINUS:
            return this->left->simul() - this->right->simul();
        case MULTIPLY:
            return this->left->simul() * this->right->simul();
        case DIVIDE:
            return round(this->left->simul() / this->right->simul());
        case ADVANTAGE:
            return std::max(this->left->simul(), this->right->simul());
        case DISADVANTAGE:
            return std::min(this->left->simul(), this->right->simul());
    }
}

Range OpTree::getRange() {
    switch (this->op) {
        case SCALAR:
            return std::make_pair(this->val, this->val);
            break;
        case DIE:
            return std::make_pair(1, this->val);
            break;
        case ROLL:
            return std::make_pair(this->left->getRange().first * this->right->getRange().first, this->left->getRange().second * this->right->getRange().second);
            break;
        case PLUS:
            return std::make_pair(this->left->getRange().first + this->right->getRange().first, this->left->getRange().second + this->right->getRange().second);
            break;
        case MINUS:
            return std::make_pair(this->left->getRange().first - this->right->getRange().second, this->left->getRange().second - this->right->getRange().first);
            break;
        case MULTIPLY:
            return std::make_pair(this->left->getRange().first * this->right->getRange().first, this->left->getRange().second * this->right->getRange().second);
            break;
        case DIVIDE:
            return std::make_pair(floor(this->left->getRange().first / this->right->getRange().second), ceil(this->left->getRange().second / this->right->getRange().first));
            break;
        case ADVANTAGE:
            return std::make_pair(std::max(this->left->getRange().first, this->right->getRange().first), std::max(this->left->getRange().second, this->right->getRange().second));
            break;
        case DISADVANTAGE:
            return std::make_pair(std::min(this->left->getRange().first, this->right->getRange().first), std::min(this->left->getRange().second, this->right->getRange().second));
            break;
    }
}

Dist OpTree::getDist(int num_sims) {
    Dist dist;
    dist.range = this->getRange();
    std::cout << dist.range.first << " " << dist.range.second << std::endl;
    dist.prob.resize(dist.getSize());
    std::fill(dist.prob.begin(), dist.prob.end(), 0);
    for (int i = 0; i < num_sims; i++) {
        dist.prob[this->simul()-dist.range.first]++;
    }
    return dist;
}

OpTree* constructOpTree(std::string command) {
    std::string word = "";
    std::string cword = "";
    std::vector<Op> op_stack;
    std::vector<OpTree*> tree_stack;
    for (int i = 0; i < command.size(); i++) {
        char c = command[i];
        if (c == ' ' || c == '[' || c == ']' || i == command.size() - 1) {
            word = cword;
            cword = "";
            if (word == "") {
                continue;
            } else if (word == "adv") {
                op_stack.push_back(ADVANTAGE);
            } else if (word == "dis") {
                op_stack.push_back(DISADVANTAGE);
            } else if (word == "+") {
                op_stack.push_back(PLUS);
            } else if (word == "-") {
                op_stack.push_back(MINUS);
            } else if (word == "*") {
                op_stack.push_back(MULTIPLY);
            } else if (word == "/") {
                op_stack.push_back(DIVIDE);
            } else if (word == "%") {
                op_stack.push_back(ROLL);
            } else {
                if (word[0] == 'd') {
                    word = word.substr(1);
                    try {
                        int val = std::stoi(word);
                        tree_stack.push_back(new OpTree(val, DIE));
                    } catch (std::invalid_argument) {
                        throw std::invalid_argument("Invalid command: " + word);
                    }
                } else {
                    try {
                        int val = std::stoi(word);
                        tree_stack.push_back(new OpTree(val, SCALAR));
                    } catch (std::invalid_argument) {
                        throw std::invalid_argument("Invalid command: " + word);
                    }
                }
            }
            if (c == '[') {
                continue;
            } else if (c == ']') {
                //pop 1 element from op_stack and two from tree_stack, make into new tree and push back onto tree_stack
                Op op = op_stack.back();
                op_stack.pop_back();
                OpTree* right = tree_stack.back();
                tree_stack.pop_back();
                OpTree* left = tree_stack.back();
                tree_stack.pop_back();
                tree_stack.push_back(new OpTree(op, left, right));    
            }
        }
        if (c != ' ' && c != '[' && c != ']') {
            cword += c;
        }
    }
    return tree_stack[0];
}