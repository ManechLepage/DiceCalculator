#include "dicecalc.hpp"
#include <iostream>

constexpr unsigned int MAX_ITER = 1000;
constexpr double threshold = 0.2;
constexpr unsigned int EVAL_SIMS = 100000;

typedef std::vector<int> Genome;

Genome anneal(Dist target, int size);