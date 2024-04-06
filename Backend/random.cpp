#include <cstdint>

static uint64_t mcg_state = 0xcafef00dd15ea5e5u;	// Must be odd
static uint64_t const multiplier = 6364136223846793005u;

[[nodiscard]] uint32_t pcg32_fast(void) {
	uint64_t x = mcg_state;
	unsigned count = (unsigned)(x >> 61);	// 61 = 64 - 3

	mcg_state = x * multiplier;
	x ^= x >> 22;
	return (uint32_t)(x >> (22 + count));	// 22 = 32 - 3 - 7
}

void pcg32_fast_init(uint64_t seed) {
	mcg_state = 2 * seed + 1;
	(void)pcg32_fast();
}

unsigned short random(const unsigned short max) {
	return pcg32_fast() % max;
}
