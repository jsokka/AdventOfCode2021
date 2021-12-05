namespace AdventOfCode2021.Puzzles
{
    internal class Day05 : IPuzzle
    {
        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day05.txt"))
                .Select(l =>
                {
                    var parts = l.Split("->");
                    var start = parts[0];
                    var end = parts[1];

                    return new Line(
                        new Point(int.Parse(start.Split(',')[0]), int.Parse(start.Split(',')[1])),
                        new Point(int.Parse(end.Split(',')[0]), int.Parse(end.Split(',')[1]))
                    );
                }).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private void Part1(List<Line> lines)
        {
            lines = lines.Where(l => l.Start.X == l.End.X || l.Start.Y == l.End.Y).ToList();

            var pointsCovered = lines.SelectMany(l => l.GetPointsCovered());

            var result = pointsCovered
                .GroupBy(p => new { p.X, p.Y })
                .Count(grp => grp.Count() > 1);

            Console.WriteLine($"Part 1: {result}");
        }

        private void Part2(List<Line> lines)
        {
            var pointsCovered = lines.SelectMany(l => l.GetPointsCovered());

            var result = pointsCovered
                .GroupBy(p => new { p.X, p.Y })
                .Count(grp => grp.Count() > 1);

            Console.WriteLine($"Part 2: {result}");
        }

        private sealed record Point(int X, int Y);

        private sealed record Line(Point Start, Point End)
        {
            public IEnumerable<Point> GetPointsCovered()
            {
                int x = Start.X;
                int y = Start.Y;

                var deltaX = 0;
                var deltaY = 0;

                if (Start.X != End.X)
                {
                    deltaX = (Start.X > End.X) ? -1 : 1;
                }

                if (Start.Y != End.Y)
                {
                    deltaY = (Start.Y > End.Y) ? -1 : 1;
                }

                while (true)
                {
                    yield return new Point(x, y);

                    if ((deltaX != 0 && x == End.X) || (deltaY != 0 && y == End.Y))
                    {
                        break;
                    }

                    x += deltaX;
                    y += deltaY;
                }
            }
        }
    }
}
