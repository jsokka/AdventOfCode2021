using System.Text;

namespace AdventOfCode2021.Puzzles
{
    internal class Day14 : IPuzzle
    {
        private string template = "";
        private Dictionary<string, char> pairMap = new();

        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day14.txt")).ToList();

            template = inputData[0];
            pairMap = inputData.Skip(2).Select(d =>
            {
                var parts = d.Split("->");
                return new { Key = parts[0].Trim(), Value = Convert.ToChar(parts[1].Trim()) };
            }).ToDictionary(d => d.Key, d => d.Value);

            Part1();
            Part2();
        }

        private void Part1()
        {
            var polymer = BruteForcePolymer(template, 10);
            var result = polymer.GroupBy(p => p).Max(grp => grp.Count()) - polymer.GroupBy(p => p).Min(grp => grp.Count());

            Console.WriteLine($"Part 1: {result}");
        }

        private void Part2()
        {
            var letters = GetPolymerLetterCounts(template, 40);
            var result = letters.Max(grp => grp.Value) - letters.Min(grp => grp.Value);

            Console.WriteLine($"Part 2: {result}");
        }

        private string BruteForcePolymer(string template, int intervals)
        {
            var polymer = new StringBuilder(template);

            for (int i = 0; i < intervals; i++)
            {
                for (int j = polymer.Length - 1; j > 0; j--)
                {
                    var pair = polymer[j - 1].ToString() + polymer[j].ToString();
                    polymer.Insert(j, pairMap[pair]);
                }
            }

            return polymer.ToString();
        }

        private Dictionary<char, long> GetPolymerLetterCounts(string template, int intervals)
        {
            var letterCounts = template.GroupBy(c => c).ToDictionary(grp => grp.Key, grp => grp.LongCount());
            var letterPairs = new Dictionary<string, long>();

            for (int i = 1; i < template.Length; i++)
            {
                var pair = template[i - 1].ToString() + template[i].ToString();
                letterPairs.AppendCount(pair, 1);
            }

            for (int i = 0; i < intervals; i++)
            {
                letterPairs = AppendLetterCounts(letterCounts, letterPairs);
            }

            return letterCounts;
        }

        private Dictionary<string, long> AppendLetterCounts(Dictionary<char, long> letterCounts, Dictionary<string, long> letterPairs)
        {
            var newPairs = new Dictionary<string, long>();

            foreach (var pair in letterPairs)
            {
                var newLetter = pairMap[pair.Key];
                letterCounts.AppendCount(newLetter, pair.Value);

                var left = pair.Key[0].ToString() + newLetter.ToString();
                var right = newLetter.ToString() + pair.Key[1].ToString();

                newPairs.AppendCount(left, pair.Value);
                newPairs.AppendCount(right, pair.Value);
            }

            return newPairs;
        }
    }

    public static class DictionaryExtensions
    {
        public static void AppendCount<T>(this IDictionary<T, long> dict, T key, long value) where T : notnull
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = 0;
            }

            dict[key] += value;
        }
    }
}
