using Microsoft.Extensions.Logging;
using Moq;
using QuestingEngine.Models;
using QuestingEngine.Repositories;
using QuestingEngine.Services;

namespace QuestEngine.Test
{
    [TestClass]
    public class QuestServiceTests
    {
        private Mock<IQuestRepository> _mockRepository;
        private Mock<ILogger<QuestService>> _mockLogger;
        private Mock<IQuestConfigService> _mockQuestConfigService;
        private QuestConfig _mockQuestConfig;
        private QuestService _questService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IQuestRepository>();
            _mockLogger = new Mock<ILogger<QuestService>>();
            _mockQuestConfigService = new Mock<IQuestConfigService>();

            // Mock QuestConfig
            _mockQuestConfig = new QuestConfig
            {
                TotalQuestPoints = 1000,
                RateFromBet = 0.1m,
                LevelBonusRate = 5m,
                Milestones = new List<Milestone>
                {
                    new Milestone { MilestoneIndex = 1, RequiredPoints = 250, ChipsAwarded = 50 },
                    new Milestone { MilestoneIndex = 2, RequiredPoints = 500, ChipsAwarded = 100 },
                    new Milestone { MilestoneIndex = 3, RequiredPoints = 750, ChipsAwarded = 200 },
                    new Milestone { MilestoneIndex = 4, RequiredPoints = 1000, ChipsAwarded = 500 }
                }
            };

            // Mock `GetQuestConfig()` to return the mocked config
            _mockQuestConfigService.Setup(q => q.GetConfig()).Returns(_mockQuestConfig);

            // Initialize QuestService with Mocks
            _questService = new QuestService(_mockRepository.Object, _mockLogger.Object, _mockQuestConfigService.Object);
        }

        [TestMethod]
        public async Task ProgressQuestAsync_NewPlayer_CreatesStateAndAccumulatesPoints()
        {
            // Arrange
            var request = new QuestProgressRequest { PlayerId = "player1", PlayerLevel = 10, ChipAmountBet = 100 };
            _mockRepository.Setup(r => r.GetPlayerQuestStateAsync(It.IsAny<string>())).ReturnsAsync((PlayerQuestState)null);
            _mockRepository.Setup(r => r.UpdatePlayerQuestStateAsync(It.IsAny<PlayerQuestState>())).Returns(Task.CompletedTask);

            // Act
            var result = await _questService.ProgressQuestAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(60, result.QuestPointsEarned); // accumulated points + total previous points = ((100 * 0.1) + (10 * 5)) + 0 = 60
            Assert.AreEqual(6, result.TotalQuestPercentCompleted); // (60 / 1000) * 100 = 6%
            _mockRepository.Verify(r => r.UpdatePlayerQuestStateAsync(It.IsAny<PlayerQuestState>()), Times.Once);
        }

        [TestMethod]
        public async Task ProgressQuestAsync_ExistingPlayer_AccumulatesPointsAndMilestoneCompleted()
        {
            // Arrange
            var existingState = new PlayerQuestState { PlayerId = "player1", Points = 240, LastMilestoneCompleted = 0 };
            var request = new QuestProgressRequest { PlayerId = "player1", PlayerLevel = 10, ChipAmountBet = 100 };

            _mockRepository.Setup(r => r.GetPlayerQuestStateAsync("player1")).ReturnsAsync(existingState);
            _mockRepository.Setup(r => r.UpdatePlayerQuestStateAsync(It.IsAny<PlayerQuestState>())).Returns(Task.CompletedTask);

            // Act
            var result = await _questService.ProgressQuestAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(300, result.QuestPointsEarned); // accumulated points + total previous points = ((100 * 0.1) + (10 * 5)) + 240 = 300
            Assert.AreEqual(300, existingState.Points); // Previous 240 + 60 = 300
            Assert.AreEqual(1, result.MilestonesCompleted.Count); // Passed first milestone
            Assert.AreEqual(1, result.MilestonesCompleted[0].MilestoneIndex);
            _mockRepository.Verify(r => r.UpdatePlayerQuestStateAsync(It.IsAny<PlayerQuestState>()), Times.Once);
        }

        [TestMethod]
        public async Task GetQuestStateAsync_PlayerExists_ReturnsCorrectState()
        {
            // Arrange
            var playerState = new PlayerQuestState { PlayerId = "player1", Points = 500, LastMilestoneCompleted = 2 };

            _mockRepository.Setup(r => r.GetPlayerQuestStateAsync("player1")).ReturnsAsync(playerState);

            // Act
            var result = await _questService.GetQuestStateAsync("player1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.TotalQuestPercentCompleted); // (500 / 1000) * 100 = 50%
            Assert.AreEqual(2, result.LastMilestoneIndexCompleted);
        }

        [TestMethod]
        public async Task GetQuestStateAsync_PlayerNotFound_ReturnsDefaultValues()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetPlayerQuestStateAsync("unknown")).ReturnsAsync((PlayerQuestState)null);

            // Act
            var result = await _questService.GetQuestStateAsync("unknown");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalQuestPercentCompleted);
            Assert.AreEqual(-1, result.LastMilestoneIndexCompleted);
        }
    }
}