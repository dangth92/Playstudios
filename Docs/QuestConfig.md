@@ -1,67 +0,0 @@
﻿# **Quest Configuration Documentation**

## **Overview**
The `questConfig.json` file defines the rules and structure of the quest system, including how points are earned and milestones are rewarded.

---

## **Configuration Properties**

### **1. TotalQuestPoints**
- **Type**: `integer`
- **Description**: The total number of points required to complete the entire quest.
- **Example**: `1000` (Players need 1000 points to complete the quest)

### **2. RateFromBet**
- **Type**: `decimal`
- **Description**: The percentage of the player's bet that is converted into quest points.
- **Example**: `0.1` (10% of the bet amount is added as quest points)
- **Formula**:  
  ```plaintext
  QuestPoints = ChipAmountBet * RateFromBet

### **3. LevelBonusRate**
- **Type**: `decimal`
- **Description**: 5 (Each level grants 5 additional quest points)
- **Example**: `0.1` (10% of the bet amount is added as quest points)
- **Formula**:  
  ```plaintext
  BonusPoints = PlayerLevel * LevelBonusRate

### **4. Milestones**
A list of milestones that define thresholds for rewards.

## Milestone Properties

### MilestoneIndex
- **Type:** `integer`
- **Description:** The sequential index of the milestone (1, 2, 3, etc.).

### RequiredPoints
- **Type:** `integer`
- **Description:** The number of quest points required to reach this milestone.

### ChipsAwarded
- **Type:** `integer`
- **Description:** The number of chips the player is rewarded upon reaching the milestone.


## 📂 Configuration File Location

The quest configuration is stored in:  
**[`Config/questConfig.json`](../QuestingEngine/Config/questConfig.json)**

## 📄 Example `questConfig.json`

```json
{
  "TotalQuestPoints": 1000,
  "RateFromBet": 0.1,
  "LevelBonusRate": 5.0,
  "Milestones": [
    { "MilestoneIndex": 1, "RequiredPoints": 250, "ChipsAwarded": 50 },
    { "MilestoneIndex": 2, "RequiredPoints": 500, "ChipsAwarded": 100 },
    { "MilestoneIndex": 3, "RequiredPoints": 750, "ChipsAwarded": 200 },
    { "MilestoneIndex": 4, "RequiredPoints": 1000, "ChipsAwarded": 500 }
  ]
}
