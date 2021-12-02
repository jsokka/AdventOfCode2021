namespace AdventOfCode2021.Puzzles
{
    internal class Day02 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day02.txt"))
                .Select(command => command.Split(' '))
                .Select(command => new KeyValuePair<string, int>(command[0], int.Parse(command[1])))
                .ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<KeyValuePair<string, int>> inputData)
        {
            var position = 0;
            var depth = 0;

            foreach (var command in inputData)
            {
                switch (command.Key)
                {
                    case "forward":
                        position += command.Value;
                        break;
                    case "up":
                        depth -= command.Value;
                        break;
                    case "down":
                        depth += command.Value;
                        break;
                }
            }

            Console.WriteLine($"Part 1: {position * depth}");
        }

        private void Part2(List<KeyValuePair<string, int>> inputData)
        {
            var depth = 0;
            var position = 0;
            var aim = 0;

            foreach (var command in inputData)
            {
                switch (command.Key)
                {
                    case "forward":
                        position += command.Value;
                        depth += command.Value * aim;
                        break;
                    case "up":
                        aim -= command.Value;
                        break;
                    case "down":
                        aim += command.Value;
                        break;
                }
            }

            Console.WriteLine($"Part 2: {position * depth}");
        }
    }
}
