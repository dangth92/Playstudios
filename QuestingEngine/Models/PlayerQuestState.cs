using System.ComponentModel.DataAnnotations;

namespace QuestingEngine.Models
{
    public class PlayerQuestState
    {
        [Key] // PlayerId is the Primary Key
        public string PlayerId { get; set; }

        public decimal Points { get; set; }
        public int LastMilestoneCompleted { get; set; }
    }
}
