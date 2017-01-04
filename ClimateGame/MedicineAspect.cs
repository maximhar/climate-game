using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class MedicineAspect : IAspect
    {
        private DependentVariable<double> cancerK;
        private DependentVariable<double> birthsK;

        public void Initialize()
        {
            cancerK = World.Instance.DependencyManager.GetDouble("Cancer.K");
            birthsK = World.Instance.DependencyManager.GetDouble("Births.K");

            // slowly cure cancer
            cancerK.AttachSource(dv => dv.LastValue * 0.999);

            // slowly reduce birthrate
            birthsK.AttachSource(dv => dv.LastValue * 1.001);
        }

        public void Tick()
        {

        }
    }
}
