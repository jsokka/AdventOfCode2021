namespace AdventOfCode2021.Puzzles
{
    internal class Day06 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<int>("Day06.txt", ",")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<int> input)
        {
            const int Days = 80;

            var fishes = new List<int>(input);

            for (int i = 0; i < Days; i++)
            {
                var newFishes = new List<int>();

                for (int j = 0; j < fishes.Count; j++)
                {
                    if (fishes[j] == 0)
                    {
                        newFishes.Add(8);
                        fishes[j] = 6;
                    }
                    else
                    {
                        fishes[j]--;
                    }
                }

                fishes.AddRange(newFishes);
            }

            Console.WriteLine($"Part 1: {fishes.Count}");
        }

        private void Part2(List<int> input)
        {
            const int Days = 256;

            var fishCounts = new long[9];
            for (int i = 0; i < input.Count; i++)
            {
                fishCounts[input[i]]++;
            }

            for (int i = 0; i < Days; i++)
            {
                var newFishes = fishCounts[0];
                for (int j = 0; j < fishCounts.Length - 1; j++)
                {
                    fishCounts[j] = fishCounts[j + 1];
                }

                fishCounts[8] = newFishes;
                fishCounts[6] += newFishes;
            }

            Console.WriteLine($"Part 2: {fishCounts.Sum()}");
        }
    }
}
