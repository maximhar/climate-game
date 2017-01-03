using System;

namespace ClimateGame
{
    interface IPopulationModifier
    {
        String Name { get; }
        Generation ModifyGeneration(Generation gen);
    }
}