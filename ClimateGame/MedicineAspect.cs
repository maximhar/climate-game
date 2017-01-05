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
        private DependentVariable<double> heartK;
        private DependentVariable<double> respiratoryK;
        private DependentVariable<double> neuralK;

        public void Initialize()
        {
            var dm = World.Instance.DependencyManager;
            cancerK = dm.GetDouble(Mix(Cancer, ParamO));
            heartK = dm.GetDouble(Mix(Heart, ParamO));
            respiratoryK = dm.GetDouble(Mix(Respiratory, ParamO));
            neuralK = dm.GetDouble(Mix(Nervous, ParamO));

            // slowly cure all diseases
            cancerK.AttachSource(dv => dv.LastValue + 0.2);
            heartK.AttachSource(dv => dv.LastValue + 0.3);
            respiratoryK.AttachSource(dv => dv.LastValue + 0.15);
            neuralK.AttachSource(dv => dv.LastValue + 0.05);
        }

        public void Tick()
        {

        }
    }
}
