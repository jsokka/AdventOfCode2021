namespace AdventOfCode2021.Puzzles
{
    internal class Day07 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<int>("Day07.txt", ",")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<int> inputData)
        {
            var (Position, Consumption) = CalculateMostEfficientPosition(inputData, true);

            Console.WriteLine($"Part 1: {Position} -> {Consumption}");
        }

        private void Part2(List<int> inputData)
        {
            var (Position, Consumption) = CalculateMostEfficientPosition(inputData, false);

            Console.WriteLine($"Part 2: {Position} -> {Consumption}");
        }

        private (int Position, int Consumption) CalculateMostEfficientPosition(List<int> inputData, bool constantConsumptionRate)
        {
            var min = inputData.Min();
            var max = inputData.Max();

            var positionFuelConsumptions = Enumerable.Range(min, max - min + 1)
                .ToDictionary(c => c, _ => 0);

            foreach (var crab in inputData)
            {
                for (int i = 0; i < positionFuelConsumptions.Count; i++)
                {
                    var consumption = Math.Abs(crab - i);

                    if (!constantConsumptionRate)
                    {
                        var temp = (consumption + 1) * (consumption / 2);
                        consumption = consumption % 2 == 0 ? temp : temp + (int)Math.Ceiling(consumption / 2m);
                    }

                    positionFuelConsumptions[i] += consumption;
                }
            }

            var result = positionFuelConsumptions.OrderBy(c => c.Value).First();

            return (result.Key, result.Value);
        }
    }
}
