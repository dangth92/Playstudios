using Newtonsoft.Json;
using QuestingEngine.Models;
using QuestingEngine.Repositories;

namespace QuestingEngine.Services
{
    public class QuestService : IQuestService
    {
        private readonly IQuestRepository _questRepository;
        private readonly ILogger<QuestService> _logger;
        private readonly QuestConfig _questConfig;

        public QuestService(IQuestRepository questRepository, ILogger<QuestService> logger, IQuestConfigService questConfigService)
        {
            _questRepository = questRepository;
            _logger = logger;
            _questConfig = questConfigService.GetConfig() ?? throw new ArgumentNullException(nameof(questConfigService));
        }

        public async Task<QuestProgressResponse> ProgressQuestAsync(QuestProgressRequest request)
        {
            _logger.LogInformation($"Processing quest progress for PlayerId: {request.PlayerId}");

            var playerState = await _questRepository.GetPlayerQuestStateAsync(request.PlayerId)
                             ?? new PlayerQuestState { PlayerId = request.PlayerId, Points = 0, LastMilestoneCompleted = 0 };

            decimal pointsEarned = (request.ChipAmountBet * _questConfig.RateFromBet) + (request.PlayerLevel * _questConfig.LevelBonusRate);
            playerState.Points += (int)pointsEarned;
            decimal percentCompleted = (playerState.Points / _questConfig.TotalQuestPoints) * 100;

            List<MilestoneCompleted> milestonesCompleted = new();
            foreach (var milestone in _questConfig.Milestones.Where(m => m.MilestoneIndex >= playerState.LastMilestoneCompleted && playerState.Points >= m.RequiredPoints))
            {
                milestonesCompleted.Add(new MilestoneCompleted { MilestoneIndex = milestone.MilestoneIndex, ChipsAwarded = milestone.ChipsAwarded });
                playerState.LastMilestoneCompleted = milestone.MilestoneIndex;
            }

            await _questRepository.UpdatePlayerQuestStateAsync(playerState);

            _logger.LogInformation($"Quest progress updated for PlayerId: {request.PlayerId}, Points: {playerState.Points}");

            return new QuestProgressResponse { QuestPointsEarned = pointsEarned, TotalQuestPercentCompleted = percentCompleted, MilestonesCompleted = milestonesCompleted };
        }

        public async Task<QuestStateResponse> GetQuestStateAsync(string playerId)
        {
            var playerState = await _questRepository.GetPlayerQuestStateAsync(playerId);
            return playerState != null
                ? new QuestStateResponse { TotalQuestPercentCompleted = (playerState.Points / _questConfig.TotalQuestPoints) * 100, LastMilestoneIndexCompleted = playerState.LastMilestoneCompleted }
                : new QuestStateResponse { TotalQuestPercentCompleted = 0, LastMilestoneIndexCompleted = -1 };
        }
    }
}
