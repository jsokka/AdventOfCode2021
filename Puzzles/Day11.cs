namespace AdventOfCode2021.Puzzles
{
    internal class Day11 : IPuzzle
    {
        private int[][] inputData = Array.Empty<int[]>();

        public async Task Solve()
        {
            await Part1();
            await Part2();
        }

        private async Task Part1()
        {
            await InitInputData();

            var totalFlashes = CalculateFlashes(100).totalFlashes;

            Console.WriteLine($"Part 1: {totalFlashes}");
        }

        private async Task Part2()
        {
            await InitInputData();

            var step = CalculateFlashes(null).steps;

            Console.WriteLine($"Part 2: {step}");
        }

        private async Task InitInputData()
        {
            inputData = (await InputDataReader.GetInputDataAsync<string>("Day11.txt"))
            .Select(row => row.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();
        }

        private (int totalFlashes, int steps) CalculateFlashes(int? stepCount, bool printSteps = false)
        {
            var totalFlashes = 0;

            for (int step = 0; step < (stepCount ?? int.MaxValue); step++)
            {
                var flashes = IncreaseAll().ToList();

                totalFlashes += flashes.Count;

                while (true)
                {
                    var surroundings = flashes.SelectMany(f => GetSurroundingValues(f.row, f.col)).ToList();

                    flashes = Increase(surroundings, true).ToList();

                    if (flashes.Count == 0)
                    {
                        break;
                    }

                    if (!stepCount.HasValue && inputData.All(row => row.All(col => col == 0)))
                    {
                        return (totalFlashes, step + 1);
                    }

                    totalFlashes += flashes.Count;
                }

                if (printSteps)
                {
                    Console.WriteLine($"Step {step + 1}:");
                    inputData.ToList().ForEach(row => Console.WriteLine(string.Join("", row)));
                    Console.WriteLine();
                }
            }

            return (totalFlashes, stepCount ?? int.MaxValue);
        }

        private IEnumerable<(int row, int col)> IncreaseAll()
        {
            for (int row = 0; row < inputData.Length; row++)
            {
                for (int col = 0; col < inputData[row].Length; col++)
                {
                    foreach (var flash in Increase(new[] { (row, col) }))
                    {
                        yield return flash;
                    }
                }
            }
        }

        private IEnumerable<(int row, int col)> Increase(IEnumerable<(int row, int col)> locations, bool expanding = false)
        {
            foreach (var (row, col) in locations)
            {
                var value = inputData[row][col];

                if (expanding && value == 0)
                {
                    continue;
                }

                if (value == 9)
                {
                    inputData[row][col] = 0;
                    yield return (row, col);
                }
                else
                {
                    inputData[row][col]++;
                }
            }
        }

        private IEnumerable<(int Row, int Col)> GetSurroundingValues(int row, int col)
        {
            if (row > 0)
            {
                yield return (row - 1, col);

                if (col > 0)
                {
                    yield return (row - 1, col - 1);
                }

                if (col < inputData.Length - 1)
                {
                    yield return (row - 1, col + 1);
                }
            }

            if (col > 0)
            {
                yield return (row, col - 1);
            }

            if (col < inputData[row].Length - 1)
            {
                yield return (row, col + 1);
            }

            if (row < inputData.Length - 1)
            {
                yield return (row + 1, col);

                if (col > 0)
                {
                    yield return (row + 1, col - 1);
                }

                if (col < inputData.Length - 1)
                {
                    yield return (row + 1, col + 1);
                }
            }
        }
    }
}
