using System.Diagnostics;

namespace AdventOfCode2021.Puzzles
{
    internal class Day12 : IPuzzle
    {
        List<string> inputData = new();

        public async Task Solve()
        {
            inputData = (await InputDataReader.GetInputDataAsync<string>("Day12.txt")).ToList();

            Part1();
            Part2();
        }

        private void Part1()
        {
            var startNode = BuildGraph(inputData);

            var paths = CalculatePaths(startNode, new List<Node>(), false);

            Console.WriteLine($"Part 1: {paths.Count}");

            //paths.ForEach(p => Console.WriteLine(string.Join(", ", p)));
        }

        private void Part2()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var startNode = BuildGraph(inputData);

            var paths = CalculatePaths(startNode, new List<Node>(), true);

            Console.WriteLine($"Part 2: {paths.Count}");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            //paths.ForEach(p => Console.WriteLine(string.Join(", ", p)));
        }

        private static Node BuildGraph(List<string> inputData)
        {
            var nodes = new HashSet<Node>();

            foreach (var connection in inputData)
            {
                var parts = connection.Split("-");

                var left = EnsureNodeExists(nodes, parts[0]);
                var right = EnsureNodeExists(nodes, parts[1]);

                left.AddConnection(right);
                right.AddConnection(left);
            }

            return nodes.Single(n => n.Name == "start");
        }

        private static Node EnsureNodeExists(HashSet<Node> nodes, string name)
        {
            var node = nodes.SingleOrDefault(n => n.Name == name);

            if (node == null)
            {
                node = new Node(name);
                nodes.Add(node);
            }

            return node;
        }

        private List<List<Node>> CalculatePaths(Node startNode, List<Node> currentPath, bool allowSingleDoubleSmall)
        {
            var paths = new List<List<Node>>();

            currentPath.Add(startNode);

            foreach (var node in startNode.ConnectedNodes)
            {
                if (node.Name == "end")
                {
                    paths.Add(currentPath.Concat(new[] { node }).ToList());
                }
                else
                {
                    var proceed = true;

                    if (node.IsSmall)
                    {
                        if (allowSingleDoubleSmall)
                        {
                            proceed = !currentPath.Where(n => n.IsSmall).GroupBy(n => n).Any(grp => grp.Count() > 1) || !currentPath.Contains(node);
                        }
                        else
                        {
                            proceed = !currentPath.Contains(node);
                        }
                    }

                    if (proceed)
                    {
                        paths.AddRange(CalculatePaths(node, new List<Node>(currentPath), allowSingleDoubleSmall));
                    }
                }
            }

            return paths;
        }

        private sealed class Node
        {
            public string Name { get; }

            public bool IsSmall { get; }

            public HashSet<Node> ConnectedNodes { get; }

            public Node(string name)
            {
                Name = name;
                IsSmall = name.All(char.IsLower);
                ConnectedNodes = new HashSet<Node>();
            }

            public void AddConnection(Node node)
            {
                if (node == null || node.Name == "start")
                {
                    return;
                }

                ConnectedNodes.Add(node);
            }
        }
    }
}
