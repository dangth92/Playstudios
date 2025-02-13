using System.ComponentModel.DataAnnotations.Schema;

namespace QuestingEngine.Models
{
    public class QuestConfig
    {
        ////public int Id { get; set; } = -1;
        public decimal TotalQuestPoints { get; set; } // Total points needed to complete the quest
        public decimal RateFromBet { get; set; } // Multiplier for ChipAmountBet
        public decimal LevelBonusRate { get; set; } // Multiplier for PlayerLevel

        [NotMapped] // Ignore this property in EF Core
        public List<Milestone> Milestones { get; set; } // List of milestone configurations
    }
}
