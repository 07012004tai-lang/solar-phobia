namespace SolarPhobia.Domain
{
    public readonly struct KindnessScore
    {
        public int Value { get; }

        public KindnessScore(int value)
        {
            Value = value;
        }

        public KindnessScore Add(int delta) => new KindnessScore(Value + delta);
    }
}

