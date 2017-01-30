using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class EconomicState
    {
        private const double GovernmentDebtInterestMultiplier = 0.1;

        public double PTI { get; set; }
        public double PTC { get; set; }

        public double PrivateNetDebt { get; set; }

        public double GovernmentNetDebt { get; set; }
        public double GovernmentDebtInterestRate => Math.Max((GovernmentNetDebt / GDP), 0) * GovernmentDebtInterestMultiplier;

        public double Employment { get; set; }
        public double Inflation => Math.Max((AggregateDemand - AggregateSupply) / AggregateSupply, -1);

        public double ProductionCapacity { get; set; }
        public double InfrastructureModifier { get; set; }
        public double EducationModifier { get; set; }
        public double TechnologyModifier { get; set; }

        public double WorkerProductivity =>
            ProductionCapacity * EducationModifier * InfrastructureModifier * TechnologyModifier;

        public double WorkingPopulation => 
            World.Instance.DependencyManager.GetDouble(Names.WorkingPopulation);

        public double GovernmentTaxation =>
            World.Instance.DependencyManager.GetDouble(Names.GovernmentTaxation);

        public double GovernmentExpenditure =>
            World.Instance.DependencyManager.GetDouble(Names.GovernmentExpenditure);

        public double GovernmentSpendingEfficiency =>
            World.Instance.DependencyManager.GetDouble(Names.GovernmentSpendingEfficiency);

        public double MaximumAggregateSupply => WorkingPopulation * WorkerProductivity;
        public double AggregateSupply => WorkingPopulation * Employment * WorkerProductivity;
        public double AggregateDemand { get; set; }

        public double GDP => AggregateDemand * (1 - Inflation);

        public EconomicState(double aggregateDemand, double employment, double prodCapacity, 
            double infModifier, double eduModifier, double techModifier,
            double pti, double ptc, double privateNetDebt = 0, double governmentNetDebt = 0)
        {
            AggregateDemand = aggregateDemand;
            Employment = employment;
            ProductionCapacity = prodCapacity;
            InfrastructureModifier = infModifier;
            EducationModifier = eduModifier;
            TechnologyModifier = techModifier;
            PTI = pti;
            PTC = ptc;
            PrivateNetDebt = privateNetDebt;
            GovernmentNetDebt = governmentNetDebt;
        }

        public EconomicState(EconomicState other) : 
            this(other.AggregateDemand, other.Employment, other.ProductionCapacity, 
                other.InfrastructureModifier, other.EducationModifier, other.TechnologyModifier,
                other.PTI, other.PTC, other.PrivateNetDebt, other.GovernmentNetDebt)
        {
        }
    }

    class EconomyAspect : IAspect
    {
        private const double PtiBase = 0.2;
        private const double PtcBase = 0.8;

        private const double InvestmentEffectOnSupply = 0.01;
        private const double NonperformingLoansRate = 0.01;

        private const int EmploymentChangeDownwardDivisor = 3;
        private const int EmploymentChangeUpwardDivisorLow = 2;
        private const int EmploymentChangeUpwardDivisorMedium = 3;
        private const int EmploymentChangeUpwardDivisorHigh = 4;
        private const int EmploymentChangeUpwardDivisorVeryHigh = 6;
        private const int EmploymentChangeUpwardDivisorExtreme = 12;

        private GaussianRandom gaussian = new GaussianRandom();

        private EconomicState currentState;

        public void Initialize()
        {
            var dm = World.Instance.DependencyManager;

            var gdpDV = dm.CreateDouble(Names.GDP);
            gdpDV.AttachSource(dv => currentState.GDP);

            var debtDV = dm.CreateDouble(Names.PrivateNetDebt);
            debtDV.AttachSource(dv => currentState.PrivateNetDebt);

            var inflationDV = dm.CreateDouble(Names.Inflation);
            inflationDV.AttachSource(dv => currentState.Inflation);

            var employmentDV = dm.CreateDouble(Names.Employment);
            employmentDV.AttachSource(dv => currentState.Employment);

            var ptiDV = dm.CreateDouble(Names.PTI);
            ptiDV.AttachSource(dv => currentState.PTI);

            var ptcDV = dm.CreateDouble(Names.PTC);
            ptcDV.AttachSource(dv => currentState.PTC);

            var governmentDebtDV = dm.CreateDouble(Names.GovernmentNetDebt);
            governmentDebtDV.AttachSource(dv => currentState.GovernmentNetDebt);

            var governmentDebtInterestDV = dm.CreateDouble(Names.GovernmentDebtInterestRate);
            governmentDebtInterestDV.AttachSource(dv => currentState.GovernmentDebtInterestRate);

            gdpDV.History = new ValueHistory<double>(10);

            // Start with demand and supply in balance.
            currentState = new EconomicState(0, 0.50, 1000, 1, 1, 1, PtiBase, PtcBase);
            currentState.AggregateDemand = currentState.AggregateSupply;
        }

        public void Tick()
        {
            EconomicState newState = new EconomicState(currentState);

            // Calculate demand, using the previous demand figure as income.
            newState.AggregateDemand = TickAggregateDemand(newState);

            // Allow employment to adjust to the gap between demand and supply.
            newState.Employment = TickEmployment(currentState);
            
            // PTC and PTI lag behind the general economy providing some inertia.
            // Calculate them now so they can be used in the next tick.
            newState.PTC = TickPropensityToConsume(currentState);

            newState.PTI = TickPropensityToInvest(newState, currentState);

            // Real net debt is scaled by inflation too.
            newState.PrivateNetDebt *= (1 - newState.Inflation);
            newState.GovernmentNetDebt *= (1 - newState.Inflation);

            currentState = newState;
        }

        private double TickPropensityToInvest(EconomicState newState, EconomicState oldState)
        {
            double deterministicFactor = 0;
            // Inflation that is too high will dissuade investment
            if (newState.Inflation > 0.10 && newState.Inflation < 0.20)
                deterministicFactor = (newState.Inflation - 0.10) * -1;
            // Extreme inflation really hammers down the point that inflation is bad for investment
            else if (newState.Inflation >= 0.20)
                deterministicFactor = (newState.Inflation - 0.16) * -2;
            // A moderate inflation will encourage investment
            else if (newState.Inflation > 0.01)
                deterministicFactor = (newState.Inflation - 0.01);
            // Deflation will discourage investment
            else if (newState.Inflation < -0.02)
                deterministicFactor = (newState.Inflation + 0.02) * 0.5;

            if (newState.Employment < 0.50)
                deterministicFactor += 0.02;
            else if (newState.Employment < 0.30)
                deterministicFactor += 0.03;

            // Negative net debt implies lower interest rates, influencing investment positively
            if (newState.PrivateNetDebt < 0)
            {
                double gdpDebt = newState.PrivateNetDebt / newState.GDP;
                deterministicFactor += gdpDebt * -0.2;
            }
            // Debt over 90% of GDP influences investment negatively according to studies
            else if (newState.PrivateNetDebt / newState.GDP > 0.9)
                deterministicFactor -= 0.02;

            double randomFactor = (gaussian.NextDouble() - 0.5) * 0.01;

            double ptcFactor = 0;
            double ptcDelta = newState.AggregateDemand - oldState.AggregateDemand;
            ptcDelta = ptcDelta / oldState.AggregateDemand;

            if (ptcDelta > 0)
                ptcFactor = ptcDelta * 2;
            else
                ptcFactor = ptcDelta;

            return Math.Max(Math.Min(PtiBase + PtiBase * deterministicFactor + PtiBase * randomFactor + PtiBase * ptcFactor, 1), 0);
        }

        private double TickPropensityToConsume(EconomicState state)
        {
            double deterministicFactor = 0;
            // Mild inflation influences consumption positively 
            if (state.Inflation > 0.02 && state.Inflation < 0.06)
                deterministicFactor = (state.Inflation - 0.01);
            // High inflation influences consumption negatively as assets decrease in value
            else if (state.Inflation >= 0.06 && state.Inflation < 0.20)
                deterministicFactor = (state.Inflation - 0.06) * -1;
            // When inflation is extreme, punish PTC even more
            else if (state.Inflation >= 0.20)
                deterministicFactor = (state.Inflation - 0.12) * -2;
            // Deflation influences consumption negatively as consumers hoard wealth
            else if (state.Inflation < -0.02)
                deterministicFactor = (state.Inflation + 0.02) * 0.5;

            // Negative net debt implies lower interest rates, influencing consumption positively
            if (state.PrivateNetDebt < 0)
                deterministicFactor += 0.02;
            // Debt over 90% of GDP influences consumption negatively according to studies
            else if (state.PrivateNetDebt / state.GDP > 0.9)
                deterministicFactor -= 0.02;

            // Random fluctuations in demand, on a gaussian distribution
            double randomFactor = (gaussian.NextDouble() - 0.5) * 0.03;

            return Math.Max(Math.Min(PtcBase + PtcBase*deterministicFactor + PtcBase*randomFactor, 1), 0);
        }

        private double TickEmployment(EconomicState state)
        {
            double demandSupplyDelta = (state.AggregateDemand - state.AggregateSupply) / state.MaximumAggregateSupply;

            double employmentChange = 0;

            // Change downwards is usually easier as it essentially represents layoffs.
            if (demandSupplyDelta < 0)
            {
                employmentChange = demandSupplyDelta / EmploymentChangeDownwardDivisor;
            }
            // Change upwards is more nuanced. Depending on the supply of labour, it will become 
            // more difficult.
            else if (demandSupplyDelta > 0)
            {
                if (state.Employment < 0.50)
                    employmentChange = demandSupplyDelta / EmploymentChangeUpwardDivisorLow;
                else if (state.Employment < 0.70)
                    employmentChange = demandSupplyDelta / EmploymentChangeUpwardDivisorMedium;
                else if (state.Employment < 0.80)
                    employmentChange = demandSupplyDelta / EmploymentChangeUpwardDivisorHigh;
                else if (state.Employment < 0.90)
                    employmentChange = demandSupplyDelta / EmploymentChangeUpwardDivisorVeryHigh;
                else
                    employmentChange = demandSupplyDelta / EmploymentChangeUpwardDivisorExtreme;
            }

            double employment = state.Employment + employmentChange;

            // Restrict employment to 0 < Employment < 1.
            return Math.Min(Math.Max(employment, 0), 1);
        }
        
        private double TickAggregateDemand(EconomicState state)
        {
            // Allow investments from last year to expand production.
            double privateIncome = state.AggregateDemand * (1 - state.GovernmentTaxation);
            double governmentIncome = state.AggregateDemand * state.GovernmentTaxation;

            double investment = privateIncome * state.PTI;
            double consumption = privateIncome * state.PTC;

            // Affect the maximum aggregate supply using a modifier.
            state.ProductionCapacity *= 
                1 + ((investment / state.MaximumAggregateSupply) * InvestmentEffectOnSupply);

            // Calculate private debt servicing (nonperforming loans only, as the rest
            // are effectively already part of the economic flow)
            double privateDebtServicing =
                Math.Max(state.PrivateNetDebt, 0) * NonperformingLoansRate;

            // Calculate government debt servicing
            double govtDebtServicing = 
                Math.Max(state.GovernmentNetDebt, 0) * state.GovernmentDebtInterestRate;
            
            // Calculate private expenditure.
            double privateExpenditure = consumption + investment - privateDebtServicing;

            // Calculate government expenditure.
            double governmentExpenditure = 
                (state.GovernmentExpenditure * state.AggregateDemand) - govtDebtServicing;

            // Process government debt
            double governmentBalance = governmentExpenditure - governmentIncome;
            state.GovernmentNetDebt += governmentBalance;

            // Process private debt
            double privateBalance = privateExpenditure - privateIncome;
            // Non-performing debts get removed from the debt ledger.
            state.PrivateNetDebt -= privateDebtServicing;
            state.PrivateNetDebt += privateBalance;

            // Aggregate demand is the sum of government (efficiency weighed) and private expenditure.
            return privateExpenditure + 
                state.GovernmentSpendingEfficiency * governmentExpenditure;
        }
    }
}
