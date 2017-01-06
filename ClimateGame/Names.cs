using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    public class Names
    {
        public const string PopulationModifiers = "PopulationModifiers";
        public const string Cancer = "Cancer";
        public const string Respiratory = "Respiratory";
        public const string Heart = "Heart";
        public const string Nervous = "Nervous";
        public const string RoadAccidents = "RoadAccidents";
        public const string WorkAccidents = "WorkAccidents";
        public const string Childhood = "Childhood";
        public const string Crime = "Crime";
        public const string Birth = "Birth";
        public const string ParamK = "K";
        public const string ParamK2 = "K2";
        public const string ParamO = "O";

        public const string ChildPopulation = "ChildPopulation";
        public const string WorkingPopulation = "WorkingPopulation";
        public const string ElderlyPopulation = "ElderlyPopulation";


        public static string Mix(string parent, string child)
        {
            return parent + "." + child;
        }
    }
}
