using Microsoft.EntityFrameworkCore;
using QuestingEngine.Models;

namespace QuestingEngine.Data
{
    public class QuestDbContext : DbContext
    {
        public QuestDbContext(DbContextOptions<QuestDbContext> options) : base(options) { }
        public DbSet<PlayerQuestState> PlayerQuestStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerQuestState>().HasData(
                new PlayerQuestState { PlayerId = "1", Points = 150, LastMilestoneCompleted = 0 },
                new PlayerQuestState { PlayerId = "2", Points = 400, LastMilestoneCompleted = 1 },
                new PlayerQuestState { PlayerId = "3", Points = 950, LastMilestoneCompleted = 2 }
            );
        }
    }
}
