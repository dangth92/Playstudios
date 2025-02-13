# 🎮 Play Studios Quest System

This project implements the **Quest System** for Play Studios, allowing players to earn quest points and rewards based on their gameplay activities.

## 🛠 Installation

Ensure you have the following tools installed on your development machine:

- .NET SDK (version 8 or above)  
- Visual Studio  
- SQL Express ([Download Here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads))  

## 🚀 Getting Started

### 1️⃣ Setup
- Clone the repository  
- Install dependencies  
- Configure `questConfig.json` under `/Config`

### 2️⃣ Running the Application
To start the API using .NET CLI, run:
```sh
dotnet run --project src/QuestEngine
```  
## 🚀 Features

- Track player progress in quests.
- Calculate quest points based on bets and player level.
- Reward players upon reaching milestones.
- API endpoints for retrieving and updating quest progress.

## 📂 Project Structure
/Config ├── questConfig.json # Quest system configuration

/docs ├── questConfig.md # Documentation for quest configuration

/src ├── QuestEngine/ # Core logic and services ├── QuestEngine.Test/ # Unit tests ├── QuestEngine.IntegrationTest/ # Integration tests

## 📌 API Endpoints
- `GET /api/quest/state?playerId={playerId}` → Get quest state  
- `POST /api/quest/progress` → Update quest progress  
- `POST /api/quest/reset` → Reset quest progress  

## 🛠️ Configuration
Edit questConfig.json to adjust quest settings.  
For detailed quest configuration, see [`Docs/QuestConfig.md`](Docs/QuestConfig.md)

## Sequence Diagram
The sequence diagram illustrating the quest process can be found here:  
[`Quest Sequence Diagram`](Docs/Quest_Sequence_Diagram.png)

## Schema
See the full schema documentation: [`Docs/Schema.md`](Docs/Schema.md)

---

© Play Studios | Built with .NET & Entity Framework Core