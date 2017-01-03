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
        private UInt32 year = 2000;

        private World()
        { }

        public static World Instance
        {
            get { return instance; }
        }

        public UInt32 Year
        {
            get { return year; }
            private set { year = value; }
        }

        public void Initialize()
        {
            populationAspect.Initialize();
        }

        public void Tick()
        {
            Year++;

            populationAspect.Tick();
        }
    }
}
