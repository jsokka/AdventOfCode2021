using System.Text;

namespace AdventOfCode2021.Puzzles
{
    internal class Day16 : IPuzzle
    {
        public async Task Solve()
        {
            var binary = string.Concat((await InputDataReader.GetInputDataAsync<string>("Day16.txt")).First()
                .Select(hex => Convert.ToString(Convert.ToInt32(hex.ToString(), 16), 2).PadLeft(4, '0')));

            Part1(binary);
            Part2(binary);
        }

        private void Part1(string binary)
        {
            var packet = Packet.ReadPacket(binary);

            Console.WriteLine($"Part 1: {SumVersions(packet)}");
        }

        private void Part2(string binary)
        {
            var packet = Packet.ReadPacket(binary);

            Console.WriteLine($"Part 1: {packet.Value}");
        }

        private int SumVersions(Packet packet)
        {
            var sum = packet.Version;

            if (packet is OperatorPacket operatorPacket)
            {
                foreach (var op in operatorPacket.SubPackets)
                {
                    sum += SumVersions(op);
                }
            }

            return sum;
        }

        private abstract class Packet
        {
            protected enum PacketType
            {
                Sum = 0,
                Product = 1,
                Min = 2,
                Max = 3,
                Literal = 4,
                GreaterThan = 5,
                LessThan = 6,
                EqualTo = 7
            }

            protected const int HeaderLength = 6;

            public int Version { get; }

            protected PacketType Type { get; }

            public virtual long Value { get; protected set; }

            public virtual int NumberOfBits { get; protected set; }

            protected Packet(string binary)
            {
                Version = Convert.ToInt32(binary.Substring(0, HeaderLength / 2), 2);
                Type = (PacketType)Convert.ToInt32(binary.Substring(HeaderLength / 2, HeaderLength / 2), 2);
            }

            public static Packet ReadPacket(string binary)
            {
                var type = (PacketType)Convert.ToInt32(binary.Substring(3, 3), 2);

                if (type == PacketType.Literal)
                {
                    return new LiteralPacket(binary);
                }
                else
                {
                    return new OperatorPacket(binary);
                }
            }
        }

        private sealed class LiteralPacket : Packet
        {
            public LiteralPacket(string binary) : base(binary)
            {
                var literalSectionLength = 0;
                var bits = new StringBuilder();

                foreach (var batch in binary[HeaderLength..].Batch(5))
                {
                    literalSectionLength += batch.Count();
                    bits.Append(batch.Skip(1).ToArray());

                    if (batch.First() == '0')
                    {
                        break;
                    }
                }

                var literal = bits.ToString();

                Value = Convert.ToInt64(literal, 2);
                NumberOfBits = HeaderLength + literalSectionLength;
            }
        }

        private sealed class OperatorPacket : Packet
        {
            public IEnumerable<Packet> SubPackets { get; }

            public int LengthType { get; }

            public override int NumberOfBits =>
                HeaderLength + 1 + (LengthType == 0 ? 15 : 11) + SubPackets.Sum(p => p.NumberOfBits);

            public override long Value
            {
                get
                {
                    switch (Type)
                    {
                        case PacketType.Sum:
                            return SubPackets.Sum(p => p.Value);
                        case PacketType.Product:
                            return SubPackets.Select(p => p.Value).Aggregate((v1, v2) => v1 * v2);
                        case PacketType.Min:
                            return SubPackets.Min(p => p.Value);
                        case PacketType.Max:
                            return SubPackets.Max(p => p.Value);
                        case PacketType.GreaterThan:
                            return SubPackets.ElementAt(0).Value > SubPackets.ElementAt(1).Value ? 1 : 0;
                        case PacketType.LessThan:
                            return SubPackets.ElementAt(0).Value < SubPackets.ElementAt(1).Value ? 1 : 0;
                        case PacketType.EqualTo:
                            return SubPackets.ElementAt(0).Value == SubPackets.ElementAt(1).Value ? 1 : 0;
                        default:
                            return 0;
                    }
                }
            }

            public OperatorPacket(string binary) : base(binary)
            {
                LengthType = Convert.ToInt32(binary.Substring(HeaderLength, 1), 2);

                int headerAndMetadataLength = HeaderLength + 1;
                int limit;

                if (LengthType == 0)
                {
                    headerAndMetadataLength += 15;
                    limit = Convert.ToInt32(binary.Substring(headerAndMetadataLength - 15, 15), 2);
                }
                else
                {
                    headerAndMetadataLength += 11;
                    limit = Convert.ToInt32(binary.Substring(headerAndMetadataLength - 11, 11), 2);
                }

                SubPackets = ReadSubPackets(binary[(headerAndMetadataLength )..], limit);
            }

            private IEnumerable<Packet> ReadSubPackets(string binary, int limit)
            {
                var packets = new List<Packet>();

                while (true)
                {
                    var packet = ReadPacket(binary);
                    packets.Add(packet);

                    if (LengthType == 0 && packets.Sum(p => p.NumberOfBits) == limit)
                    {
                        break;
                    }

                    if (LengthType == 1 && packets.Count == limit)
                    {
                        break;
                    }

                    binary = binary[packet.NumberOfBits..];
                }

                return packets;
            }
        }
    }

    public static class MyExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int batchSize)
        {
            return items.Select((item, i) => new { item, i })
                .GroupBy(item => item.i / batchSize)
                .Select(grp => grp.Select(grp => grp.item));
        }
    }
}
