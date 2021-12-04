using System.Text;

namespace AdventOfCode2021.Puzzles
{
    internal class Day03 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day03.txt")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<string> inputData)
        {
            var length = inputData[0].Length;

            var gammaRate = new StringBuilder();
            var epsilonRate = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                var mostCommon = GetMostCommonValue(inputData, i);

                gammaRate.Append(mostCommon);
                epsilonRate.Append(mostCommon == '0' ? '1' : '0');
            }

            Console.WriteLine($"Part 1: {Convert.ToInt32(gammaRate.ToString(), 2) * Convert.ToInt32(epsilonRate.ToString(), 2)}");
        }

        private void Part2(List<string> inputData)
        {
            var length = inputData[0].Length;
            var oxygenRatingCadidates = new List<string>(inputData);
            var scrubberRatingCadidates = new List<string>(inputData);

            for (int i = 0; i < length; i++)
            {
                if (oxygenRatingCadidates.Count > 1)
                {
                    var mostCommon = GetMostCommonValue(oxygenRatingCadidates, i) ?? '1';
                    oxygenRatingCadidates.RemoveAll(val => val[i] != mostCommon);
                }

                if (scrubberRatingCadidates.Count > 1)
                {
                    var leastCommon = GetMostCommonValue(scrubberRatingCadidates, i);
                    leastCommon = leastCommon == '0' ? '1' : '0'; 
                    scrubberRatingCadidates.RemoveAll(val => val[i] != leastCommon);
                }

                if (oxygenRatingCadidates.Count > 1 && scrubberRatingCadidates.Count > 1 && i == length - 1)
                {
                    i = 0;
                }
            }

            Console.WriteLine($"Part 2: {Convert.ToInt32(oxygenRatingCadidates.Single(), 2) * Convert.ToInt32(scrubberRatingCadidates.Single(), 2)}");
        }

        private static char? GetMostCommonValue(List<string> inputData, int index)
        {
            var q = inputData.Select(s => s[index])
                .GroupBy(c => c)
                .Select(grp => new { Value = grp.Key, Count = grp.Count() });

            if (q.All(grp => grp.Count == q.First().Count))
            {
                return null;
            }

            return q.Where(grp => grp.Count == q.Max(grp2 => grp2.Count)).Select(grp => grp.Value).Single();
        }
    }
}
