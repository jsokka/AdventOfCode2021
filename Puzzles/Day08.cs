namespace AdventOfCode2021.Puzzles
{
    internal class Day08 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day08.txt")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<string> inputData)
        {
            var result = inputData.Select(v => v.Split(" | ")[1]).SelectMany(s => s.Split(' '))
                .Where(s => new[] { 2, 3, 4, 7 }.Contains(s.Length));

            Console.WriteLine($"Part 1: {result.Count()}");
        }

        private void Part2(List<string> inputData)
        {
            var result = 0;

            foreach (var display in inputData)
            {
                var parts = display.Split(" | ");
                var signalPattern = parts[0].Split(' ');
                var output = parts[1].Split(' ').Select(d => string.Join("", d.OrderBy(c => c)));

                var numbers = GetNumbers(signalPattern);

                var outpuValue = int.Parse(string.Join("",
                    output.Select(d => numbers.Single(kvp => kvp.Value == d).Key)));
                result += outpuValue;
            }

            Console.WriteLine($"Part 2: {result}");
        }

        private Dictionary<int, string> GetNumbers(IEnumerable<string> signalPatterns)
        {
            var numbers = new Dictionary<int, string>
            {
                [1] = signalPatterns.Single(d => d.Length == 2),
                [4] = signalPatterns.Single(d => d.Length == 4),
                [7] = signalPatterns.Single(d => d.Length == 3),
                [8] = signalPatterns.Single(d => d.Length == 7)
            };

            numbers[3] = signalPatterns.Single(d => d.Length == 5 && d.Contains(numbers[1][0]) && d.Contains(numbers[1][1]));
            numbers[6] = signalPatterns.Single(d => d.Length == 6 && (d.Contains(numbers[1][0]) ^ d.Contains(numbers[1][1])));
            numbers[5] = signalPatterns.Single(d => d.Length == 5 && numbers[6].Except(d).Count() == 1);
            numbers[2] = signalPatterns.Single(d => d.Length == 5 && !numbers.ContainsValue(d));
            numbers[9] = signalPatterns.Single(d => d.Length == 6 && d.Contains(numbers[4][0]) && d.Contains(numbers[4][1]) && d.Contains(numbers[4][2]) && d.Contains(numbers[4][3]));
            numbers[0] = signalPatterns.Single(d => !numbers.ContainsValue(d));

            return numbers.ToDictionary(d => d.Key, d => string.Join("", d.Value.OrderBy(c => c)));
        }
    }
}
