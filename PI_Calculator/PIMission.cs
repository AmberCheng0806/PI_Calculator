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
        public PIMission(int sampleSize) { SampleSize = sampleSize; }
        public async Task<(long, double)> Calculate()
        {
            object key = new object();
            int size = 1000;
            int count = (int)Math.Ceiling((double)SampleSize / size);
            //Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int circleNum = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Parallel.ForAsync(0, count, (i, token) =>
            {
                if (i + 1 == count)
                {
                    size = SampleSize - i * size;
                }
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                for (int j = 0; j < size; j++)
                {
                    double x = rnd.NextDouble();
                    double y = rnd.NextDouble();
                    if (Math.Pow(x, 2) + Math.Pow(y, 2) < 1) lock (key) circleNum++;
                }
                return ValueTask.CompletedTask;
            });
            stopwatch.Stop();
            return (stopwatch.ElapsedMilliseconds, Math.Round(4.0 * circleNum / SampleSize, 3));
        }

    }
}
