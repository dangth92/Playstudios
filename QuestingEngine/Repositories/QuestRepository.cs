using Microsoft.EntityFrameworkCore;
using QuestingEngine.Data;
using QuestingEngine.Models;

namespace QuestingEngine.Repositories
{
    public class QuestRepository : IQuestRepository
    {
        private readonly QuestDbContext _context;

        public QuestRepository(QuestDbContext context)
        {
            _context = context;
        }

        public async Task<PlayerQuestState> GetPlayerQuestStateAsync(string playerId)
        {
            var playerState = await _context.PlayerQuestStates.FirstOrDefaultAsync(p => p.PlayerId == playerId);

            return playerState;
        }

        public async Task UpdatePlayerQuestStateAsync(PlayerQuestState state)
        {
            var existingState = await _context.PlayerQuestStates.FindAsync(state.PlayerId);

            if (existingState == null)
            {
                // If the player state does not exist, insert it
                state.LastMilestoneCompleted = 0;
                await _context.PlayerQuestStates.AddAsync(state);
            }
            else
            {
                // Update existing player state
                existingState.Points = state.Points;
                existingState.LastMilestoneCompleted = state.LastMilestoneCompleted;
                _context.PlayerQuestStates.Update(existingState);
            }

            await _context.SaveChangesAsync();
        }
    }
}
