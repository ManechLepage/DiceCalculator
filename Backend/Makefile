CC=g++
LDFLAGS=
CFLAGS=-I. -Wall -Wextra -std=c++11 -g -O0

all: program

program: main.o dicecalc.o random.o io.o search.o
	$(CC) $(LDFLAGS) main.o dicecalc.o random.o io.o search.o -o dicecalc.exe

main.o: main.cpp dicecalc.cpp io.cpp
	$(CC) $(CFLAGS) -c main.cpp

search.o: search.cpp dicecalc.cpp random.cpp
	$(CC) $(CFLAGS) -c search.cpp

dicecalc.o: dicecalc.cpp random.cpp
	$(CC) $(CFLAGS) -c dicecalc.cpp

random.o: random.cpp
	$(CC) $(CFLAGS) -c random.cpp

io.o: io.cpp dicecalc.cpp
	$(CC) $(CFLAGS) -c io.cpp

clean:
	rm -f *.o program