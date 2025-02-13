
using Microsoft.EntityFrameworkCore;
using QuestingEngine.Data;
using QuestingEngine.Models;
using QuestingEngine.Repositories;
using QuestingEngine.Services;
using System;

namespace QuestingEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Register QuestConfigService for dependency injection
            builder.Services.AddSingleton<IQuestConfigService, QuestConfigService>();

            // Register db context
            builder.Services.AddDbContext<QuestDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IQuestRepository, QuestRepository>();
            builder.Services.AddScoped<IQuestService, QuestService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enable Logging
            builder.Services.AddLogging();

            var app = builder.Build();

            // Create and seed the database
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<QuestDbContext>();
                dbContext.Database.EnsureCreated(); // Creates database and seeds data
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
