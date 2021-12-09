namespace AdventOfCode2021.Puzzles
{
    internal class Day09 : IPuzzle
    {
        private int[][] inputData = Array.Empty<int[]>();
        private readonly List<(int Row, int Col)> locationsHandled = new();

        public async Task Solve()
        {
            inputData = (await InputDataReader.GetInputDataAsync<string>("Day09.txt"))
                .Select(row => row.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();

            Part1();
            Part2();
        }

        private void Part1()
        {
            var riskLevel = GetLowPoints().Select(lp => lp.Value + 1).Sum();

            Console.WriteLine($"Part 1: {riskLevel}");
        }

        private void Part2()
        {
            var lowPoints = GetLowPoints();

            var basins = new List<int>();

            foreach (var (Row, Col, Value) in lowPoints)
            {
                basins.Add(GetBasinSize(Row, Col));
            }

            var result = basins.OrderByDescending(b => b).Take(3).Aggregate((x, y) => x * y);

            Console.WriteLine($"Part 2: {result}");
        }

        private IEnumerable<(int Row, int Col, int Value)> GetLowPoints()
        {
            for (int row = 0; row < inputData.Length; row++)
            {
                for (int col = 0; col < inputData[row].Length; col++)
                {
                    var surroundingValues = GetSurroundingValues(row, col);
                    var currentValue = inputData[row][col];

                    if (surroundingValues.All(val => val.Value > currentValue))
                    {
                        yield return (row, col, currentValue);
                    }
                }
            }
        }

        private IEnumerable<(int Row, int Col, int Value)> GetSurroundingValues(int row, int col)
        {
            if (row > 0)
            {
                yield return (row - 1, col, inputData[row - 1][col]);
            }

            if (col > 0)
            {
                yield return (row, col - 1, inputData[row][col - 1]);
            }

            if (col < inputData[row].Length - 1)
            {
                yield return (row, col + 1, inputData[row][col + 1]);
            }

            if (row < inputData.Length - 1)
            {
                yield return (row + 1, col, inputData[row + 1][col]);
            }
        }

        private int GetBasinSize(int row, int col)
        {
            if (inputData[row][col] == 9 || locationsHandled.Any(l => l.Row == row && l.Col == col))
            {
                return 0;
            }

            locationsHandled.Add((row, col));

            var surroundingValues = GetSurroundingValues(row, col);

            var size = 1;

            foreach (var (Row, Col, Value) in surroundingValues)
            {
                size += GetBasinSize(Row, Col);
            }

            return size;
        }
    }
}
