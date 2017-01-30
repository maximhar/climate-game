using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class GovernmentAspect : IAspect
    {
        public double Expenditure { get; set; } // as fraction of national income
        public double Taxation { get; set; } // as fraction of national income

        // Higher efficiency means greater spending power in real terms.
        private double governmentSpendingEfficiency = 0.8;

        public void Initialize()
        {
            var dm = World.Instance.DependencyManager;

            var expenditureDV = dm.CreateDouble(Names.GovernmentExpenditure)
                .AttachSource(dv => Expenditure);

            var taxationDV = dm.CreateDouble(Names.GovernmentTaxation)
                .AttachSource(dv => Taxation);

            var efficiencyDV = dm.CreateDouble(Names.GovernmentSpendingEfficiency)
                .AttachSource(dv => governmentSpendingEfficiency);
        }

        public void Tick()
        {
        }
    }
}
