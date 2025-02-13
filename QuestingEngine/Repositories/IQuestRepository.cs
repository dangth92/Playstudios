using QuestingEngine.Models;

namespace QuestingEngine.Repositories
{
    public interface IQuestRepository
    {
        Task<PlayerQuestState> GetPlayerQuestStateAsync(string playerId);
        Task UpdatePlayerQuestStateAsync(PlayerQuestState state);
    }
}
