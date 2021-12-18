namespace AdventOfCode2021.Puzzles
{
    internal class Day17 : IPuzzle
    {
        private int targetLeftX;
        private int targetRightX;
        private int targetTopY;
        private int targetBottomY;

        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day17.txt")).First();

            InitTargetArea(inputData);

            Part1();
            Part2();
        }

        private void Part1()
        {
            var paths = Simulate();

            Console.WriteLine($"Part 1: {paths.Max(p => p.Max(pos => pos.Item2))}");
        }

        private void Part2()
        {
            var paths = Simulate();

            Console.WriteLine($"Part 2: {paths.Count()}");
        }

        private void InitTargetArea(string inputData)
        {
            var parts = inputData.Substring(inputData.IndexOf(":") + 2).Split(',')
                .Select(p => p.Trim().TrimStart('x').TrimStart('y').TrimStart('='));

            var xParts = parts.First().Split("..").Select(int.Parse).ToArray();
            var yParts = parts.Last().Split("..").Select(int.Parse).ToArray();

            targetLeftX = xParts[0];
            targetRightX = xParts[1];
            targetTopY = yParts[1];
            targetBottomY = yParts[0];
        }

        private IEnumerable<HashSet<(int, int)>> Simulate()
        {
            for (int vx0 = 1; vx0 < 1000; vx0++)
            {
                for (int vy0 = -1000; vy0 < 1000; vy0++)
                {
                    var path = GetPath(vx0, vy0);
                    if (path.Count > 0)
                    {
                        yield return path;
                    }
                }
            }
        }

        private HashSet<(int x, int y)> GetPath(int vx0, int vy0)
        {
            var positions = new HashSet<(int x, int y)>();
            var x = 0;
            var y = 0;
            var vx = vx0;
            var vy = vy0;

            while (true)
            {
                positions.Add((x, y));

                if (IsInTargetArea(x, y))
                {
                    return positions;
                }

                if (IsOverTargetArea(x, y))
                {
                    break;
                }

                x += vx;
                y += vy;
                vy--;
                vx -= vx > 0 ? 1 : 0;
            }

            return new HashSet<(int x, int y)>();
        }

        private bool IsInTargetArea(int x, int y)
        {
            return x >= targetLeftX
                && x <= targetRightX
                && y >= targetBottomY
                && y <= targetTopY;
        }

        private bool IsOverTargetArea(int x, int y)
        {
            return x > targetRightX || y < targetBottomY;
        }
    }
}
