using QuestingEngine.Models;

namespace QuestingEngine.Services
{
    public interface IQuestService
    {
        Task<QuestProgressResponse> ProgressQuestAsync(QuestProgressRequest request);
        Task<QuestStateResponse> GetQuestStateAsync(string playerId);
    }
}
