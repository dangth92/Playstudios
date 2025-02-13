namespace QuestingEngine.Models
{
    public class QuestProgressResponse
    {
        public decimal QuestPointsEarned { get; set; }
        public decimal TotalQuestPercentCompleted { get; set; }
        public List<MilestoneCompleted> MilestonesCompleted { get; set; } = new();
    }
}
