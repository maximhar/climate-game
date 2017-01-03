using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class World : IAspect
    {
        private static readonly World instance = new World();

        private PopulationAspect populationAspect = new PopulationAspect();

        private World()
        { }

        public static World Instance
        {
            get { return instance; }
        }

        public UInt32 Year { get; private set; }

        public Stats Stats { get; private set; }

        public void Initialize()
        {
            Stats = new Stats();
            Year = 2000;
            populationAspect.Initialize();
        }

        public void Tick()
        {
            Year++;

            populationAspect.Tick();
            Stats.Print();
        }
    }
}
