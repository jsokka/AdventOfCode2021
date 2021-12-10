namespace AdventOfCode2021.Puzzles
{
    internal class Day10 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day10.txt")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<string> inputData)
        {
            var parenthesis = new List<(char open, char close, int score)>
            {
                { ('(', ')', 3) },
                { ('[', ']', 57) },
                { ('{', '}', 1197) },
                { ('<', '>', 25137) }
            };

            var totalScore = 0;

            foreach (var line in inputData)
            {
                var characters = new List<char>();

                foreach (var c in line)
                {
                    var isOpenParenthesis = parenthesis.Any(p => p.open == c);

                    if (isOpenParenthesis)
                    {
                        characters.Add(c);
                    }
                    else if (characters.Last() == parenthesis.Single(r => r.close == c).open)
                    {
                        characters.RemoveAt(characters.Count - 1);
                    }
                    else
                    {
                        totalScore += parenthesis.Single(p => p.close == c).score;
                        break;
                    }
                }
            }

            Console.WriteLine($"Part 1: {totalScore}");
        }

        private void Part2(List<string> inputData)
        {
            var parenthesis = new List<(char open, char close, int score)>
            {
                { ('(', ')', 1) },
                { ('[', ']', 2) },
                { ('{', '}', 3) },
                { ('<', '>', 4) }
            };

            var scores = new List<long>();

            foreach (var line in inputData)
            {
                var i = 0;
                var characters = new List<char>();

                foreach (var c in line)
                {
                    var isOpenParenthesis = parenthesis.Any(p => p.open == c);

                    if (isOpenParenthesis)
                    {
                        characters.Add(c);
                    }
                    else if (characters.Last() == parenthesis.Single(r => r.close == c).open)
                    {
                        characters.RemoveAt(characters.Count - 1);
                    }
                    else
                    {
                        characters.Clear();
                        break;
                    }

                    i++;
                }

                if (characters.Count == 0)
                {
                    continue;
                }

                characters.Reverse();

                //Console.WriteLine(string.Join("", characters.Select(c => parenthesis.SingleOrDefault(p => p.open == c).close)));

                long lineScore = 0;

                foreach (var c in characters)
                {
                    lineScore = (lineScore * 5) + parenthesis.Single(p => p.open == c).score;
                }

                scores.Add(lineScore);
            }

            var result = scores.OrderBy(s => s).Skip(scores.Count / 2).Take(1).First();

            Console.WriteLine($"Part 2: {result}");
        }
    }
}
