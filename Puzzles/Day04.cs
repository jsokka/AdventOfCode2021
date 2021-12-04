using System.Text.RegularExpressions;

namespace AdventOfCode2021.Puzzles
{
    internal class Day04 : IPuzzle
    {
        private void Part1(List<string> inputData)
        {
            var numbersDrawn = inputData[0].Split(',').Select(int.Parse).ToHashSet();
            var boards = GetBoards(inputData.Skip(1).ToList());

            var result = 0;

            foreach (var number in numbersDrawn)
            {
                DrawNumber(boards, number);

                var winningBoard = boards.SingleOrDefault(b => b.IsWinningBoard);

                if (winningBoard != null)
                {
                    result = winningBoard.Rows.SelectMany(v => v.Values.Where(v => !v.IsMarked)).Sum(v => v.Number) * number;
                    break;
                }
            }

            Console.WriteLine($"Part 1: {result}");
        }

        private void Part2(List<string> inputData)
        {
            var numbersDrawn = inputData[0].Split(',').Select(int.Parse).ToHashSet();
            var boards = GetBoards(inputData.Skip(1).ToList());

            var result = 0;
            var winningBoards = new List<Board>();

            foreach (var number in numbersDrawn)
            {
                DrawNumber(boards, number);

                winningBoards.AddRange(boards.Where(b => b.IsWinningBoard && !winningBoards.Contains(b)).ToList());

                if (winningBoards.Count == boards.Count)
                {
                    result = winningBoards.Last().Rows.SelectMany(v => v.Values.Where(v => !v.IsMarked)).Sum(v => v.Number) * number;
                    break;
                }
            }

            Console.WriteLine($"Part 2: {result}");
        }

        private void DrawNumber(List<Board> boards, int number)
        {
            foreach (var board in boards)
            {
                board.MarkNumbers(number);
            }
        }

        private sealed class Board
        {
            public HashSet<Row> Rows { get; set; } = new HashSet<Row>();

            public bool IsWinningBoard => GetWinnignValues().Any();

            public IEnumerable<IEnumerable<Value>> GetWinnignValues()
            {
                var rowValues = Rows.Where(r => r.Values.All(v => v.IsMarked)).Select(r => r.Values);
                var columnValues = new List<IEnumerable<Value>>();

                for (int i = 0; i < Rows.First().Values.Count; i++)
                {
                    var values = Rows.SelectMany(r => r.Values.Where(rv => rv.Index == i));
                    if (values.All(v => v.IsMarked))
                    {
                        columnValues.Add(values);
                    }
                }

                return rowValues.Concat(columnValues);
            }

            public void MarkNumbers(int number)
            {
                foreach (var value in Rows.SelectMany(r => r.Values).Where(v => v.Number == number))
                {
                    value.IsMarked = true;
                }
            }
        }

        private sealed class Row
        {
            public HashSet<Value> Values { get; set; } = new HashSet<Value>();
        }

        private sealed class Value
        {
            public int Number { get; set; }
            public int Index { get; set; }
            public bool IsMarked { get; set; }
        }

        public async Task Solve()
        {
            var inputData = (await InputDataReader.GetInputDataAsync<string>("Day04.txt")).ToList();

            Part1(inputData);
            Part2(inputData);
        }

        private List<Board> GetBoards(List<string> inputData)
        {
            var boards = new List<Board>();

            Board? board = new();

            foreach (var row in inputData)
            {
                if (row.Length == 0)
                {
                    board = new Board();
                    boards.Add(board);
                    continue;
                }

                var values = Regex.Matches(row, @"\s?\d+").Select(v => new Value
                {
                    Number = int.Parse(v.Value.Trim())
                }).ToList();

                foreach (var value in values)
                {
                    value.Index = values.IndexOf(value);
                }

                board.Rows.Add(new Row
                {
                    Values = values.ToHashSet()
                });
            }

            return boards;
        }
    }
}
