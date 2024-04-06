#include "search.hpp"

OpTree* treeFromVec(const Genome& genome) {
    unsigned int size = genome.size();
    //first elem of genome is always a scalar
    OpTree* current = new OpTree(genome[0], SCALAR);
    for (unsigned int i = 1; i < size; i++) {
        //die at pos i has i + 1 faces
        OpTree* die = new OpTree(ROLL, new OpTree(genome[i], SCALAR), new OpTree(i + 1, DIE));
        //add to tree
        current = new OpTree(PLUS, current, die);
    }
    return current;
}

//never mind, this does not work since number of die must be an integer
/* Genome fdsa(Dist target, int size) {
    Genome genome;
    Genome pos_estimate;
    Genome neg_estimate;
    genome.reserve(size);
    for (int i = 0; i < size; i++) {
        int die_max = target.range.second / (i + 1);
        genome.push_back(rand() % (die_max + 1));
    }
    //err starts out as infinity
    double err = std::numeric_limits<double>::infinity();
    int n = 0;
    while (err > threshold) {
        pos_estimate = genome;
        neg_estimate = genome;
        double cn = -0.000003 * (n * n) + 3;
        for (int i = 0; i < size; i++) {
            //add unit vector * cn to genome
            pos_estimate[i] += cn;
            neg_estimate[i] -= cn;
            double pos_err = getErr(target, treeFromVec(pos_estimate)->getDist(EVAL_SIMS));
            double neg_err = getErr(target, treeFromVec(neg_estimate)->getDist(EVAL_SIMS));

        }
        n += 1; 
    }

} */
Genome neighbour(Genome current) {
    Genome new_genome = current;
    for (unsigned int i = 0; i < current.size(); i++) {
        if (random(2)) {
            if (random(2)) {
                new_genome[i] += 1;
            } else {
                new_genome[i] -= 1;
                new_genome[i] = std::max(0, new_genome[i]);
            }
        }
    }
    return new_genome;
}

Genome anneal(Dist target, int size) {
    Genome genome; 
    genome.reserve(size);
    for (int i = 0; i < size; i++) {
        int die_max = target.range.second / (i + 1);
        genome.push_back(rand() % (die_max + 1));
    }
    double prev_err = getErr(target, treeFromVec(genome)->getDist(EVAL_SIMS));
    double temperature;
    for (unsigned int i = 0; i < MAX_ITER; i++) {
        temperature = 1 - ((i + 1) / (double)MAX_ITER);
        temperature = 2 * temperature * temperature;
        Genome new_genome = neighbour(genome);
        double err = getErr(target, treeFromVec(genome)->getDist(EVAL_SIMS));
        std::cout << "New error: " << err << std::endl;
        for (int j = 0; j < size; j++) {
            std::cout << new_genome[j] << " ";
        }
        std::cout << std::endl;
        if (err < prev_err) {
            genome = new_genome;
            prev_err = err;
        } else {
            double prob = exp(-(err - prev_err) / temperature);
            if (randomDouble() < prob) {
                genome = new_genome;
                prev_err = err;
            }
        }
        std::cout << "Iteration " << i << " with error " << prev_err << std::endl;
    }
    return genome;
}