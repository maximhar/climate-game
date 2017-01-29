using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class GaussianRandom
    {
        private Random random = new Random();

        public double NextDouble(double mean = 0, double stdDev = 1)
        {
            double r1 = random.NextDouble();
            double r2 = random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(r1)) * Math.Sin(2.0 * Math.PI * r2);
            return mean + stdDev * randStdNormal;
        }
    }
}
