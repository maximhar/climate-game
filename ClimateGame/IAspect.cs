using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    interface IAspect : ITickable
    {
        void Initialize();
    }
}
