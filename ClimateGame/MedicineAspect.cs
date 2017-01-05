using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClimateGame.Names;

namespace ClimateGame
{
    class MedicineAspect : IAspect
    {
        private DependentVariable<double> cancerK;
        private DependentVariable<double> birthsK;

        public void Initialize()
        {
            cancerK = World.Instance.DependencyManager.GetDouble(Mix(Cancer, ParamK));
            birthsK = World.Instance.DependencyManager.GetDouble(Mix(Birth, ParamK));

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
