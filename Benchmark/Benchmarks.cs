using BenchmarkDotNet.Attributes;
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

            var queue = new Queue<string>();
            queue.Enqueue("I");
            queue.Enqueue("am");
            queue.Enqueue("Matthew");
            queue.Enqueue("and");
            queue.Enqueue("I");
            queue.Enqueue("am");
            queue.Enqueue("a");
            queue.Enqueue("dev");

            while (queue.Count != 0)
            {
                data += " ";
                data += queue.Dequeue();
            }
        }

        [Benchmark]
        public void UsingStringBuilder()
        {
            var data = new StringBuilder("Hello, Sir!");

            var queue = new Queue<string>();
            queue.Enqueue("I");
            queue.Enqueue("am");
            queue.Enqueue("Matthew");
            queue.Enqueue("and");
            queue.Enqueue("I");
            queue.Enqueue("am");
            queue.Enqueue("a");
            queue.Enqueue("dev");

            while (queue.Count != 0)
            {
                data.Append(" ");
                data.Append(queue.Dequeue());
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
            var bufferLength = data.Length + concatenations.Sum(c => c.Length) + concatenations.Length;

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
