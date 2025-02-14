using Microsoft.AspNetCore.Mvc;
using QuestingEngine.Models;
using QuestingEngine.Services;

namespace QuestingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;
        private readonly ILogger<QuestController> _logger;

        public QuestController(IQuestService questService, ILogger<QuestController> logger)
        {
            _questService = questService;
            _logger = logger;
        }

        [HttpPost("progress")]
        public async Task<IActionResult> ProgressQuest([FromBody] QuestProgressRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PlayerId))
            {
                _logger.LogWarning("GetQuestState called with empty playerId.");
                return BadRequest("Player ID is required.");
            }

            var response = await _questService.ProgressQuestAsync(request);
            return Ok(response);
        }

        [HttpGet("state")]
        public async Task<IActionResult> GetQuestState([FromQuery] string playerId)
        {
            var response = await _questService.GetQuestStateAsync(playerId);

            if (response == null || response.LastMilestoneIndexCompleted == -1)
            {
                _logger.LogInformation($"No quest state found for PlayerId: {playerId}");
                return NotFound(new { Message = "Player quest state not found." });
            }

            return Ok(response);
        }
    }

}
