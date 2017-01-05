using System;

namespace ClimateGame
{
    struct AgeRange
    {
        public UInt32 StartAge { get; }
        public UInt32 EndAge { get; }
        
        public AgeRange(UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            StartAge = startAge;
            EndAge = endAge;
        }

        public bool Contains(UInt32 age)
        {
            return (age >= StartAge && age < EndAge);
        }
    }
}