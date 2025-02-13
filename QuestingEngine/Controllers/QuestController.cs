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

        public QuestController(IQuestService questService)
        {
            _questService = questService;
        }

        [HttpPost("progress")]
        public async Task<IActionResult> ProgressQuest([FromBody] QuestProgressRequest request)
        {
            var response = await _questService.ProgressQuestAsync(request);
            return Ok(response);
        }

        [HttpGet("state")]
        public async Task<IActionResult> GetQuestState([FromQuery] string playerId)
        {
            var response = await _questService.GetQuestStateAsync(playerId);

            if (response == null || response.LastMilestoneIndexCompleted == -1)
            {
                return NotFound(new { Message = "Player quest state not found." });
            }

            return Ok(response);
        }
    }

}
