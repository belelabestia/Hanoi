using System;
using System.Collections.Generic;
using System.Linq;

runGame(3);

void runGame(int diskCount)
{
    IEnumerable<int>[] towers;
    int moveCount;

    setupState();
    startGame();

    void setupState()
    {
        towers = new IEnumerable<int>[]
        {
            Enumerable.Range(1, diskCount),
            Enumerable.Empty<int>(),
            Enumerable.Empty<int>()
        };

        moveCount = 0;
    }

    void startGame()
    {
        while (true)
        {
            outputState();

            string input;

            onInput();

            int from, to;

            try
            {
                validateInput();
            }
            catch
            {
                outputInputError();
                continue;
            }

            if (validateIndexes())
            {
                outputIndexError();
                continue;
            }

            if (validateFromTowerFilled())
            {
                outputTowerError();
                continue;
            }

            if (validateMove())
            {
                outputMoveError();
                continue;
            }

            applyMove();

            if (checkGameSolved())
            {
                outputGameSolved();
                break;
            }

            void onInput()
            {
                input = Console.ReadLine()!;
                Console.WriteLine("------");
            }

            void validateInput()
            {
                var indexes = input.Split(" ");
                from = int.Parse(indexes[0]);
                to = int.Parse(indexes[1]);
            }

            void outputInputError()
            {
                Console.WriteLine("Describe your next move with a '<from> <to>' notation, where <from> and <to> are the zero-based index of the towers.");
            }

            bool validateIndexes()
            {
                return (from, to) is ( < 0 or > 2, _) or (_, < 0 or > 2);
            }

            void outputIndexError()
            {
                Console.WriteLine("Indexes must be between 0 and 2.");
            }

            bool validateFromTowerFilled()
            {
                return !towers[from].Any();
            }

            void outputTowerError()
            {
                Console.WriteLine("You cannot move a disk from an empty tower.");
            }

            bool validateMove()
            {
                return !towers[to].Any() || towers[to].Last() > towers[from].Last();
            }

            void outputMoveError()
            {
                Console.WriteLine("You cannot move a disk on a smaller disk.");
            }

            void applyMove()
            {
                towers[to] = towers[to].Append(towers[from].Last());
                towers[from] = towers[from].SkipLast(1);
                moveCount++;
            }

            bool checkGameSolved()
            {
                return !towers[0].Any() && !towers[1].Any() && towers[2].Count() == diskCount;
            }

            void outputGameSolved()
            {
                Console.WriteLine("You won the game in {0} moves!", moveCount);
            }
        }

        void outputState()
        {
            for (int i = 0; i < towers.Count(); i++)
            {
                var tower = towers[i];
                Console.WriteLine(i + " | " + string.Join(' ', tower));
            }
            Console.WriteLine("------");
        }
    }
}