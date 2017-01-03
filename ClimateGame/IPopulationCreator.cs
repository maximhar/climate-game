using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    interface IPopulationCreator
    {
        String Name { get; }
        Generation CreateGeneration(Generation gen);
    }
}
