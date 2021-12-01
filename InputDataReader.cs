using System.ComponentModel;

namespace AdventOfCode2021
{
    public static class InputDataReader
    {
        private readonly static string inputDataFolderPath =
            Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName, "InputData");

        public async static Task<IEnumerable<T>> GetInputDataAsync<T>(string fileName, string delimiter = "\n", bool trimEmptyLastLine = true)
        {
            string filePath = Path.Combine(inputDataFolderPath, fileName);

            var lines = (await File.ReadAllTextAsync(filePath))
                .Split(delimiter).ToList();

            if (trimEmptyLastLine && string.IsNullOrWhiteSpace(lines.Last()))
            {
                lines.RemoveAt(lines.Count - 1);
            }

            return lines.Select(Convert<T>);
        }

        private static T Convert<T>(string value)
        {
            var tc = TypeDescriptor.GetConverter(typeof(T));
            return (T?)tc.ConvertFromInvariantString(value.Trim()) ?? throw new ArgumentException(value);
        }
    }
}
