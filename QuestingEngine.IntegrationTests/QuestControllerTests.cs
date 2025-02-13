using QuestingEngine.Models;
using System.Net.Http.Json;

namespace QuestingEngine.IntegrationTests
{
    [TestClass]
    public class QuestControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;
        private readonly string _playerId = "player1";

        [TestInitialize]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();

            _factory.SeedTestDataAsync(_playerId, 500, 2);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [TestMethod]
        public async Task GetQuestState_ReturnsCorrectData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/quest/state?playerId={_playerId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<QuestStateResponse>();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.TotalQuestPercentCompleted); // 500 / 1000 * 100 = 50
            Assert.AreEqual(2, result.LastMilestoneIndexCompleted);
        }

        [TestMethod]
        public async Task ProgressQuestAsync_UpdatesPlayerState()
        {
            // Arrange

            var request = new QuestProgressRequest
            {
                PlayerId = _playerId,
                PlayerLevel = 5,
                ChipAmountBet = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/quest/progress", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<QuestProgressResponse>();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(35, result.QuestPointsEarned); // (100 * 0.1) + (5 * 5) = 35
            Assert.AreEqual((decimal)53.5, result.TotalQuestPercentCompleted); // (35 + 500) / 1000 * 100 = 53.5
            Assert.AreEqual(2, result.MilestonesCompleted.Count); // (35 + 500) = 535, 500 < 750 (milestone 3) => passed 2 milestones
            Assert.AreEqual(2, result.MilestonesCompleted?.LastOrDefault()?.MilestoneIndex); // (35 + 500) = 535, 500 < 535 < 750 => last milestone = 2
        }
    }
}