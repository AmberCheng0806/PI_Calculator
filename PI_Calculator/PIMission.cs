using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_Calculator
{
    internal class PIMission
    {
        public int SampleSize { get; set; }

        public CancellationToken CancellationToken { get; set; }
        public PIMission(int sampleSize, CancellationToken cancellationToken)
        {
            SampleSize = sampleSize; this.CancellationToken = cancellationToken;
        }
        public async Task<(long, double)> Calculate()
        {
            object key = new object();
            int size = 1000;
            int count = (int)Math.Ceiling((double)SampleSize / size);
            int circleNum = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Parallel.ForAsync(0, count, CancellationToken, (i, token) =>
            {
                if (token.IsCancellationRequested) return ValueTask.CompletedTask;
                if (i + 1 == count)
                {
                    size = SampleSize - i * size;
                }
                //Random rnd = new Random(Guid.NewGuid().GetHashCode());
                for (int j = 0; j < size; j++)
                {
                    double x = Random.Shared.NextDouble(); // rnd.NextDouble();
                    double y = Random.Shared.NextDouble();
                    if (Math.Pow(x, 2) + Math.Pow(y, 2) < 1) Interlocked.Increment(ref circleNum);// lock (key) circleNum++; 原子操作
                }
                return ValueTask.CompletedTask;
            });
            stopwatch.Stop();
            return (stopwatch.ElapsedMilliseconds, Math.Round(4.0 * circleNum / SampleSize, 3));
        }

    }
}
