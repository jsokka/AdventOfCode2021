namespace AdventOfCode2021.Puzzles
{
    internal class Day13 : IPuzzle
    {
        private IEnumerable<(int x, int y)> paper = Enumerable.Empty<(int x, int y)>();
        private IEnumerable<(string dir, int line)> foldInstructions = Enumerable.Empty<(string dir, int line)>();

        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day13.txt")).ToList();

            paper = inputData.Take(inputData.IndexOf("")).Select(row =>
            {
                var parts = row.Split(',');
                return (int.Parse(parts[0]), int.Parse(parts[1]));
            });

            foldInstructions = inputData.Skip(inputData.IndexOf("") + 1)
                .Select(row => (row.Substring(row.IndexOf("=") - 1, 1), int.Parse(row[(row.IndexOf("=") + 1)..])));

            Part1();
            Part2();
        }

        private void Part1()
        {
            var (direction, line) = foldInstructions.First();
            var foldedPaper = Fold(paper, direction, line);

            Console.WriteLine($"Part 1: {foldedPaper.Count()}");
        }

        private void Part2()
        {
            var paperToFold = paper;

            foreach (var (dir, line) in foldInstructions)
            {
                paperToFold = Fold(paperToFold, dir, line);
            }

            Console.WriteLine("Part 2:");
            PrintPaper(paperToFold);
        }

        private static IEnumerable<(int x, int y)> Fold(IEnumerable<(int x, int y)> paper, string direction, int line)
        {
            HashSet<(int x, int y)> foldedPaper;

            if (direction == "x")
            {
                foldedPaper = paper.Where(p => p.x < line)
                    .Concat(paper.Where(d => d.x > line).Select(p => (line - (p.x - line), p.y))).ToHashSet();
            }
            else
            {
                foldedPaper = paper.Where(p => p.y < line)
                    .Concat(paper.Where(d => d.y > line).Select(p => (p.x, line - (p.y - line)))).ToHashSet();
            }

            return foldedPaper;
        }

        private static void PrintPaper(IEnumerable<(int x, int y)> paperToPrint)
        {
            for (int row = 0; row < paperToPrint.Max(p => p.y) + 1; row++)
            {
                for (int col = 0; col < paperToPrint.Max(p => p.x) + 1; col++)
                {
                    var hasDot = paperToPrint.Any(d => d.y == row && d.x == col);
                    Console.Write(hasDot ? " # " : " . ");
                }

                Console.WriteLine();
            }
        }
    }
}
