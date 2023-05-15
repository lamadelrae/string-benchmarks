using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Text;

namespace StringBenchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        [Benchmark(Baseline = true)]
        public void UsingJustString()
        {
            var data = "Hello, Sir!";

            var concatenations = new[]
            {
                "I",
                "am",
                "Matthew",
                "and",
                "I",
                "am",
                "a",
                "dev"
            };

            for (var i = 0; i < concatenations.Length; i++)
            {
                data += ' ';
                data += concatenations[i];
            }
        }

        [Benchmark]
        public void UsingStringBuilder()
        {
            var data = new StringBuilder("Hello, Sir!");
            var concatenations = new[]
            {
                "I",
                "am",
                "Matthew",
                "and",
                "I",
                "am",
                "a",
                "dev"
            };

            for (var i = 0; i < concatenations.Length; i++)
            {
                data.Append(' ');
                data.Append(concatenations[i]);
            }
        }

        [Benchmark]
        public void UsingSpan()
        {
            var data = "Hello, Sir!".ToCharArray();
            var concatenations = new[]
            {
                "I",
                "am",
                "Matthew",
                "and",
                "I",
                "am",
                "a",
                "dev"
            };

            // Calculate buffer length
            var bufferLength = data.Length + concatenations.Length;
            for(var i = 0; i < concatenations.Length; i++) bufferLength += concatenations[i].Length;

            var buffer = new char[bufferLength];
            var bufferSpan = buffer.AsSpan();

            data.CopyTo(bufferSpan);
            var whereIsMyBufferActuallyAt = data.Length;

            for (var i = 0; i < concatenations.Length; i++)
            {
                bufferSpan[whereIsMyBufferActuallyAt] = ' ';
                whereIsMyBufferActuallyAt++;

                var unit = concatenations[i];
                unit.AsSpan().CopyTo(bufferSpan.Slice(whereIsMyBufferActuallyAt));
                whereIsMyBufferActuallyAt += unit.Length;
            }
        }
    }
}
