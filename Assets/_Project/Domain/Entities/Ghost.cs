using System.Collections.Generic;

namespace SolarPhobia.Domain
{
    public class Ghost
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public int VisitCount { get; set; }
        public int Satisfaction { get; private set; }

        public List<string> DialogueNodes { get; } = new List<string>();

        public Ghost(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
            VisitCount = 0;
            Satisfaction = 100;
        }

        public void ApplyKindness(int delta)
        {
            Satisfaction = System.Math.Clamp(Satisfaction + delta * 10, 0, 100);
            VisitCount = System.Math.Max(VisitCount, 1);
        }
    }
}

