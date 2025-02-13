using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuestingEngine.Models;

namespace QuestingEngine.Services
{
    public class QuestConfigService : IQuestConfigService
    {
        private readonly QuestConfig _questConfig;
        private readonly ILogger<QuestConfigService> _logger;

        public QuestConfigService(ILogger<QuestConfigService> logger)
        {
            _logger = logger;
            _questConfig = LoadConfig();
        }

        private QuestConfig LoadConfig()
        {
            try
            {
                string configPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "questConfig.json");

                if (!File.Exists(configPath))
                {
                    _logger.LogError($"QuestConfig file not found: {configPath}");
                    throw new FileNotFoundException("QuestConfig file not found.", configPath);
                }

                string json = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<QuestConfig>(json) ?? new QuestConfig();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading QuestConfig: {ex.Message}");
                throw;
            }
        }

        public QuestConfig GetConfig() => _questConfig;
    }
}
