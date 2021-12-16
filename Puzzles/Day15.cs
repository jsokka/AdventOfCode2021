namespace AdventOfCode2021.Puzzles
{
    internal class Day15 : IPuzzle
    {
        private readonly Dictionary<(int, int), Node> graph = new();

        public async Task Solve()
        {
            await Part1();
            await Part2();
        }

        private async Task Part1()
        {
            await InitGraph(1);

            var lowestRisk = GetLowestRiskWithDijkstra(graph.First().Value, graph.Last().Value);

            Console.WriteLine($"Part 1: {lowestRisk}");
        }

        private async Task Part2()
        {
            await InitGraph(5);

            var lowestRisk = GetLowestRiskWithDijkstra(graph.First().Value, graph.Last().Value);

            Console.WriteLine($"Part 2: {lowestRisk}");
        }

        private async Task InitGraph(int size)
        {
            var initialGraph = (await InputDataReader.GetInputDataAsync<string>("Day15.txt"))
                .SelectMany((s, y) => s.Select((c, x) => new { x, y, risk = int.Parse(c.ToString()) }))
                .ToDictionary(d => (d.x, d.y), d => new Node(d.x, d.y, d.risk));

            var gridSize = initialGraph.Max(g => g.Value.X) + 1;

            foreach (var deltaY in Enumerable.Range(0, size))
            {
                foreach (var deltaX in Enumerable.Range(0, size))
                {
                    foreach (var node in initialGraph.Values)
                    {
                        var x = node.X + (gridSize * deltaX);
                        var y = node.Y + (gridSize * deltaY);
                        var risk = node.Risk + deltaX + deltaY;
                        if (risk > 9)
                        {
                            risk -= 9;
                        }

                        graph[(x, y)] = new Node(x, y, risk);
                    }
                }
            }
        }

        private int GetLowestRiskWithDijkstra(Node sourceNode, Node targetNode)
        {
            var queue = new PriorityQueue<Node, int>();

            sourceNode.UpdateDistance(0);

            queue.Enqueue(sourceNode, sourceNode.Distance);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (node == targetNode)
                {
                    break;
                }

                if (node.Visited)
                {
                    continue;
                }

                node.Visit();

                foreach (var nextNode in GetAdjacentNodes(node))
                {
                    var cost = node.Distance + nextNode.Risk;

                    if (cost < nextNode.Distance)
                    {
                        nextNode.UpdateDistance(cost);
                        queue.Enqueue(nextNode, cost);
                    }
                }
            }

            return targetNode.Distance;
        }

        private IEnumerable<Node> GetAdjacentNodes(Node node)
        {
            foreach (var (x, y) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                var pos = (node.X + x, node.Y + y);

                if (graph.ContainsKey(pos) && !graph[pos].Visited)
                {
                    yield return graph[pos];
                }
            }
        }

        private sealed class Node
        {
            public int X { get; }

            public int Y { get; }

            public int Risk { get; }

            public int Distance { get; private set; }

            public bool Visited { get; private set; }

            public Node(int x, int y, int risk)
            {
                X = x;
                Y = y;
                Risk = risk;
                Distance = int.MaxValue;
            }

            public void UpdateDistance(int distance)
            {
                Distance = distance;
            }

            public void Visit()
            {
                Visited = true;
            }
        }
    }
}
