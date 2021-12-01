namespace AdventOfCode2021.Puzzles
{
    internal class Day01 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<int>("Day01.txt")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<int> inputData)
        {
            var increaseCount = 0;

            for (int i = 1; i < inputData.Count; i++)
            {
                increaseCount += inputData[i] - inputData[i - 1] > 0 ? 1 : 0;
            }

            Console.WriteLine($"Part 1: {increaseCount}");
        }

        private void Part2(List<int> inputData)
        {
            const int Sliding = 3;
            var prevSum = 0;
            var increaseCount = 0;

            for (int i = 0; i < inputData.Count; i++)
            {
                var sum = inputData.Skip(i).Take(Sliding).Sum();

                increaseCount += (i > 0 && sum > prevSum) ? 1 : 0;
                prevSum = sum;
            }

            Console.WriteLine($"Part 2: {increaseCount}");
        }
    }
}
