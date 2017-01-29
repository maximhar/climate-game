using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class EconomyAspect : IAspect
    {
        // Represents the overall technological level of the economy
        private double techLevel = 1;
        // Represents the overall education level of the population
        private double educationLevel = 1;
        // Represents the overall development of the country infrastructure
        private double infrastructureLevel = 1;
        // Represents the basic production capacity of the economy, increased by investment.
        private double basicCapacity = 1;

        private double gdp;

        private double debt;

        private double inflation;

        private double employment = 0.50;

        // Propensity to consume.
        private double ptc = 0.8;
        private const double ptcBase = 0.81;
        // Propensity to invest;
        private double pti = 0.2;
        private const double ptiBase = 0.2;


        private double WorkingPopulation => World.Instance.DependencyManager.GetDouble(Names.WorkingPopulation);

        private double WorkerProductivity => basicCapacity* educationLevel * infrastructureLevel* techLevel;

        private GaussianRandom gaussian = new GaussianRandom();

        private double oldDemand = -1;

        public void Initialize()
        {
            var dm = World.Instance.DependencyManager;

            var gdpDV = dm.CreateDouble(Names.GDP);
            gdpDV.AttachSource(dv => gdp);

            var debtDV = dm.CreateDouble(Names.Debt);
            debtDV.AttachSource(dv => debt);

            var inflationDV = dm.CreateDouble(Names.Inflation);
            inflationDV.AttachSource(dv => inflation);

            var employmentDV = dm.CreateDouble(Names.Employment);
            employmentDV.AttachSource(dv => employment);

            var ptiDV = dm.CreateDouble(Names.PTI);
            ptiDV.AttachSource(dv => pti);

            var ptcDV = dm.CreateDouble(Names.PTC);
            ptcDV.AttachSource(dv => ptc);

            gdpDV.History = new ValueHistory<double>(10);

            gdp = WorkingPopulation * employment * WorkerProductivity;
        }

        public void Tick()
        {
            ptc = TickPropensityToConsume(inflation);

            // Maximum aggregate supply
            double supplyMax = TickMaximumAggregateSupply();

            double demand = TickAggregateDemand();

            if (oldDemand < 0)
                oldDemand = demand;

            double supply = TickAggregateSupply(demand);

            employment = TickEmployment(demand, supply, supplyMax, employment);

            supply = TickAggregateSupply(demand);

            gdp = (demand + supply) / 2;

            inflation = TickInflation(gdp, supply);

            pti = TickPropensityToInvest(inflation, demand, oldDemand);

            gdp *= (1 - inflation);
            debt *= (1 - inflation);

            oldDemand = demand;
        }

        private double TickPropensityToInvest(double inflation, double d, double od)
        {
            double deterministicFactor = 0;
            if (inflation > 0.02)
                deterministicFactor = 0.01;
            else if (inflation < 0)
                deterministicFactor = -0.01;

            if (employment < 50)
                deterministicFactor += 0.02;
            else if (employment < 30)
                deterministicFactor += 0.03;
            else if (employment > 80)
                deterministicFactor -= 0.01;
            else if (employment > 90)
                deterministicFactor -= 0.02;

            double randomFactor = (gaussian.NextDouble() - 0.5) * 0.01;

            double ptcFactor = 0;
            double ptcDelta = d - od;
            ptcDelta = ptcDelta / od;

            if (ptcDelta > 0)
                ptcFactor = ptcDelta * 2;
            else
                ptcFactor = ptcDelta;

            return Math.Max(Math.Min(ptiBase + ptiBase * deterministicFactor + ptiBase * randomFactor + ptiBase * ptcFactor, 1), 0);
        }

        private double TickPropensityToConsume(double inflation)
        {
            double deterministicFactor = 0;
            if (inflation > 0.02 && inflation < 0.06)
                deterministicFactor = 0.01;
            else if (inflation >= 0.06)
                deterministicFactor = -0.01;
            else if (inflation < -0.02)
                deterministicFactor = -0.01;

            double randomFactor = (gaussian.NextDouble() - 0.5) * 0.03;



            return Math.Max(Math.Min(ptcBase + ptcBase*deterministicFactor + ptcBase*randomFactor, 1), 0);
        }

        private double TickInflation(double demand, double supply)
        {
            double delta = (demand - supply) / demand;
            return delta;
        }

        private double TickEmployment(double demand, double supply, double maxSupply, double employment)
        {
            double delta = (demand - supply) / maxSupply;

            double employmentChange = delta / 2;

            employment += employmentChange;

            return Math.Min(employment, 1);
        }

        private double TickAggregateSupply(double demand)
        {
            return WorkingPopulation * employment * WorkerProductivity;
        }

        private double TickAggregateDemand()
        {
            double income = gdp;
            double consumption = income * ptc;
            double investment = income * pti;

            basicCapacity += ((investment / income) * 0.01);

            double nonperformingLoans = debt * 0.01;

            double demand = consumption + investment;
            double loans = Math.Max(demand - income, 0);

            debt += loans;

            debt -= nonperformingLoans;
            debt = Math.Max(0, debt);

            demand -= nonperformingLoans;

            return consumption + investment;
        }

        private double TickMaximumAggregateSupply()
        {
            double maxAS = WorkingPopulation * WorkerProductivity;

            return maxAS;
        }
    }
}
