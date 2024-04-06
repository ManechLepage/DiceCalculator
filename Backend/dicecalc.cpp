#include "dicecalc.hpp"

double getErr(const Dist& target, const Dist& given) {
    int tighter_min = std::max(given.range.first, target.range.first);
    int looser_min = std::min(given.range.first, target.range.first);
    int tighter_max = std::min(given.range.second, target.range.second);
    int looser_max = std::max(given.range.second, target.range.second);
    double signed_err = 0;
    double err = 0;
    for (int i = looser_min; i <= looser_max; i++) {
        if (i < tighter_min) {
            if (given.range.first == looser_min) signed_err = given.prob[i-given.range.first] / (double)given.total;
            else signed_err = target.prob[i-target.range.first] / target.total;
        } else if (i > tighter_max) {
            if (given.range.second == looser_max) signed_err = given.prob[i-given.range.first] / (double)given.total;
            else signed_err = target.prob[i-target.range.first] / target.total;
        } else {
            signed_err = (given.prob[i-given.range.first] - target.prob[i-target.range.first]) / (double)(given.total + target.total);
        }
        err += signed_err * signed_err;
    }
    return err;
}

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
    return 0;
}

Range OpTree::getRange() {
    switch (this->op) {
        case SCALAR:
            return std::make_pair(this->val, this->val);
        case DIE:
            return std::make_pair(1, this->val);
        case ROLL:
            return std::make_pair(this->left->getRange().first * this->right->getRange().first, this->left->getRange().second * this->right->getRange().second);
        case PLUS:
            return std::make_pair(this->left->getRange().first + this->right->getRange().first, this->left->getRange().second + this->right->getRange().second);
        case MINUS:
            return std::make_pair(this->left->getRange().first - this->right->getRange().second, this->left->getRange().second - this->right->getRange().first);
        case MULTIPLY:
            return std::make_pair(this->left->getRange().first * this->right->getRange().first, this->left->getRange().second * this->right->getRange().second);
        case DIVIDE:
            return std::make_pair(floor(this->left->getRange().first / this->right->getRange().second), ceil(this->left->getRange().second / this->right->getRange().first));
        case ADVANTAGE:
            return std::make_pair(std::max(this->left->getRange().first, this->right->getRange().first), std::max(this->left->getRange().second, this->right->getRange().second));
        case DISADVANTAGE:
            return std::make_pair(std::min(this->left->getRange().first, this->right->getRange().first), std::min(this->left->getRange().second, this->right->getRange().second));
    }
    return std::make_pair(0, 0);
}

Dist OpTree::getDist(int num_sims) {
    Dist dist;
    dist.range = this->getRange();
    dist.prob.resize(dist.getSize());
    dist.total = num_sims;
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
    for (unsigned int i = 0; i < command.size(); i++) {
        char c = command[i];
        if (c == ' ' || c == '[' || c == ']' || i == command.size() - 1) {
            word = cword;
            cword = "";
            if (word == "") {
                ;
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
                    } catch (std::invalid_argument const&) {
                        throw std::invalid_argument("Invalid command: " + word);
                    }
                } else {
                    try {
                        int val = std::stoi(word);
                        tree_stack.push_back(new OpTree(val, SCALAR));
                    } catch (std::invalid_argument const&) {
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