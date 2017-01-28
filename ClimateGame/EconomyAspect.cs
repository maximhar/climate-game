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
        private double techLevel = 0;
        private double techAdvance;

        private double outlook = 0;

        private double interestRate = 0.00;

        private double gdp;

        private double gdpPerWorker = 10000;

        private double debt = 0;
        
        private double investment;


        public void Initialize()
        {
            var dm = World.Instance.DependencyManager;
            var workingPopulation = dm.GetDouble(WorkingPopulation);

            var gdpDV = dm.CreateDouble(GDP);
            gdpDV.AttachSource(dv => gdp);

            var debtDV = dm.CreateDouble(Debt);
            debtDV.AttachSource(dv => debt);

            gdpDV.History = new ValueHistory<double>(10);
        }

        public void Tick()
        {
            var dm = World.Instance.DependencyManager;
            var workingPopulation = dm.GetDouble(WorkingPopulation);

            techAdvance = 0.3;
            techLevel += techAdvance;

            // Semi-random investment outlook based on year
            outlook = TickOutlook();

            // Calculate investment and debt
            investment = TickInvestment();

            gdp = workingPopulation * gdpPerWorker;
        }

        private double TickOutlook()
        {
            double[] outlookCycle = { 1.0, 0.9, 0.8, 0.7, 0.7, 0.6, 0.6, 0.5, 0.5, 0.4, 0.4,
                                      0.3, 0.3, 0.3, 0.3, 0.2, 0.2, 0.2, 0.2, 0.1, 0.0,
                                     -0.1, -0.2, -0.3, -0.4, -0.5};

            Random rand = new Random();

            uint outlookIndex = World.Instance.Year + (uint)rand.Next(5);
          

            double outlook = outlookCycle[outlookIndex % (outlookCycle.Length)];
            outlook += techAdvance;
          
            return outlook * 0.05;
        }

        private double TickInvestment()
        {
            var dm = World.Instance.DependencyManager;
            var workingPopulation = dm.GetDouble(WorkingPopulation);

            // pay denbt
            double interest = Math.Max(debt, 0) * interestRate;
            debt -= interest;

            double interestPerWorker = interest / workingPopulation;
            gdpPerWorker -= interestPerWorker;


            // add investments
            double investment = outlook * gdpPerWorker;
            debt += Math.Max(investment, 0) * workingPopulation;

            gdpPerWorker += investment;

            return investment;
        }
       


    }
}
