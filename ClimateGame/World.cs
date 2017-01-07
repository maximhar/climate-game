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
        private MedicineAspect medicineAspect = new MedicineAspect();
        private EconomyAspect economyAspect = new EconomyAspect();

        private World()
        { }

        public static World Instance
        {
            get { return instance; }
        }

        public UInt32 Year { get; private set; }

        public DependencyManager DependencyManager { get; private set; }

        public Stats Stats { get; private set; }

        public void Initialize()
        {
            Stats = new Stats();
            Year = 2000;
            DependencyManager = new DependencyManager();
            populationAspect.Initialize();
            medicineAspect.Initialize();
            economyAspect.Initialize();
        }

        public void Tick()
        {
            Year++;

            populationAspect.Tick();
            medicineAspect.Tick();
            economyAspect.Tick();

            Stats.Print();
            DependencyManager.Tick();
        }
    }
}
