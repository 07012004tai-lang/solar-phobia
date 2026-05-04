using System.Collections.Generic;

namespace SolarPhobia.Domain
{
    public class DialogueNode
    {
        public string Id { get; set; }
        public string Npc { get; set; }
        public string Character { get; set; }
        public string Portrait { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public List<Choice> Choices { get; set; } = new List<Choice>();
        public string Next { get; set; }
        public List<DialogueEffect> Effects { get; set; } = new List<DialogueEffect>();
        public string Condition { get; set; }
    }

    public class Choice
    {
        public string Text { get; set; }
        public string Target { get; set; }
        public int KindnessValue { get; set; }
        public DialogueEffect Effect { get; set; }
    }

    public class DialogueEffect
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}

