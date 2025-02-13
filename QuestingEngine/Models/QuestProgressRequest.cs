namespace QuestingEngine.Models
{
    public class QuestProgressRequest
    {
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public decimal ChipAmountBet { get; set; }
    }
}
