using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClimateGame.Names;

namespace ClimateGame
{
    class EconomyAspect : IAspect
    {
        private double gdp;
        private double gdpCapita = 20000;

        public void Initialize()
        {
            var dm = World.Instance.DependencyManager;
            var workingPopulation = dm.GetDouble(WorkingPopulation);
            var gdpDV = dm.CreateDouble(GDP);
            gdpDV.AttachSource(dv => gdp);
            gdpDV.History = new ValueHistory<double>(10);
        }

        public void Tick()
        {
            var dm = World.Instance.DependencyManager;
            var workingPopulation = dm.GetDouble(WorkingPopulation);
            // Modest 2% yearly growth per capita
            gdpCapita *= 1.02;
            gdp = workingPopulation * gdpCapita;
        }
    }
}
