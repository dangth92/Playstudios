namespace QuestingEngine.Models
{
    public class Milestone
    {
        public int MilestoneIndex { get; set; } // Milestone number
        public int RequiredPoints { get; set; } // Points needed to reach this milestone
        public int ChipsAwarded { get; set; } // Rewarded chips when milestone is completed
    }
}
