using System.ComponentModel.DataAnnotations;

namespace QuestingEngine.Models
{
    public class PlayerQuestState
    {
        [Key]
        [MaxLength(50)]
        public string PlayerId { get; set; } = string.Empty;

        public decimal Points { get; set; }
        public int LastMilestoneCompleted { get; set; }
    }
}
