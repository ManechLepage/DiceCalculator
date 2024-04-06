#include <iostream>
#include "dicecalc.hpp"
int main() {
    pcg32_fast_init(23738);
    OpTree* tree = constructOpTree("[% 3 d6]");
    std::cout << tree->repr() << std::endl;
    delete tree;
    return 0;    
}