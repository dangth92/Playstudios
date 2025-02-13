using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestingEngine.Data;
using QuestingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestingEngine.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DB context (SQL Server)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<QuestDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // FORCE a New ServiceProvider to Drop SQL Server
                var newServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add InMemory database for testing
                ////services.AddDbContext<QuestDbContext>(options =>
                ////    options.UseInMemoryDatabase("TestDatabase"));
                // RE-REGISTER QuestDbContext with InMemoryDatabase
                services.AddDbContext<QuestDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                    options.UseInternalServiceProvider(newServiceProvider);
                });

                // Ensure database is created
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<QuestDbContext>();
                    
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });
        }

        public void SeedTestDataAsync(string playerId, int points, int milestone)
        {
            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<QuestDbContext>();

                db.PlayerQuestStates.Add(new PlayerQuestState
                {
                    PlayerId = playerId,
                    Points = points,
                    LastMilestoneCompleted = milestone
                });

                db.SaveChanges();
            }
        }
    }
}
