# Player Quest Progress Database Schema

This document outlines the schema of the data model that stores player quest progress.

## **PlayerQuestState Table**
This table tracks the player's quest progress.

| Column Name            | Data Type     | Description |
|------------------------|--------------|-------------|
| **PlayerId**           | `VARCHAR(50)` | Unique identifier for the player. |
| **Points**             | `DECIMAL`         | Total quest points accumulated by the player. |
| **LastMilestoneCompleted** | `INT`    | The last milestone index the player has completed. |

## **Milestone Model**
This table defines the quest milestones and their associated rewards.

| Column Name      | Data Type     | Description |
|------------------|--------------|-------------|
| **MilestoneIndex** | `INT`       | The sequential index of the milestone. |
| **RequiredPoints** | `INT`       | The number of points required to reach this milestone. |
| **ChipsAwarded**  | `INT`       | The number of chips awarded when reaching this milestone. |

## **Entity Model (C#)**
### **PlayerQuestState Entity**
```csharp
public class PlayerQuestState
{
    [Key]
    public string PlayerId { get; set; }

    public decimal Points { get; set; }

    public int LastMilestoneCompleted { get; set; }
}
```
### **PlayerQuestState Entity**
```csharp
public class Milestone
{
    public int MilestoneIndex { get; set; } // Milestone number
    public int RequiredPoints { get; set; } // Points needed to reach this milestone
    public int ChipsAwarded { get; set; } // Rewarded chips when milestone is completed
}
