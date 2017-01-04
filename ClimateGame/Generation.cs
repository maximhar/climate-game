using System;

namespace ClimateGame
{
    struct Generation
    {
        public UInt32 YearOfBirth { get; }

        public double Count { get;  }

        public UInt32 Age => World.Instance.Year - YearOfBirth;

        public Generation(UInt32 yob, double count)
        {
            YearOfBirth = yob;
            Count = count;
        }

        public Generation SetCount(double count)
        {
            return new Generation(YearOfBirth, count);
        }

        public Generation AddCount(double count)
        {
            return new Generation(YearOfBirth, Count + count);
        }
    }
}
