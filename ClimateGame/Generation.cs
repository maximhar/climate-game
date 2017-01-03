using System;

namespace ClimateGame
{
    struct Generation
    {
        public UInt32 YearOfBirth { get; }

        public Int64 Count { get;  }

        public UInt32 Age => World.Instance.Year - YearOfBirth;

        public Generation(UInt32 yob, Int64 count)
        {
            YearOfBirth = yob;
            Count = count;
        }

        public Generation SetCount(Int64 count)
        {
            return new Generation(YearOfBirth, count);
        }

        public Generation AddCount(Int64 count)
        {
            return new Generation(YearOfBirth, Count + count);
        }
    }
}
