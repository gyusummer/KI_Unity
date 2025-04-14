using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class QuestSystem : MonoBehaviour
{
    public const string CATEGORY_PLACE_BLOCK = "PlaceBlock";
    public const string CATEGORY_REMOVE_BLOCK = "RemoveBlock";
    
    public struct QuestData
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Parameter { get; set; }
    }

    public class Quest
    {
        public QuestData Data { get; set; }
        public int MaxProgress { get; set; }
        public int CurrentProgress { get; set; }
    }

    private QuestData[] questList;
    private Quest activeQuest; 
    
    private void Start()
    {
        questList = GetQuestOrNull();
        activeQuest = CreateQuest(questList[1]);
    }

    private Quest CreateQuest(QuestData data)
    {
        Quest quest = new Quest();
        quest.Data = data;
        quest.CurrentProgress = 0;

        switch (data.Category)
        {
            case CATEGORY_PLACE_BLOCK:
                int.TryParse(data.Parameter.Split("_")[1], out int placeAmount);
                quest.MaxProgress = placeAmount;
                BlockSystem.Instance.Events.OnPlaceBlock += UpdatePlaceBlockQuest;
                break;
            case CATEGORY_REMOVE_BLOCK:
                int.TryParse(data.Parameter.Split("_")[1], out int removeAmount);
                quest.MaxProgress = removeAmount;
                BlockSystem.Instance.Events.OnRemoveBlock += UpdateRemoveBlockQuest;
                break;
            default:
                break;
        }

        Debug.Log($"Quest Created! ::: {quest.Data.Name}, {quest.CurrentProgress}/{quest.MaxProgress}");
        
        return quest;
    }

    private void CompleteQuest(Quest quest)
    {
        switch (quest.Data.Category)
        {
            case CATEGORY_PLACE_BLOCK:
                BlockSystem.Instance.Events.OnPlaceBlock -= UpdatePlaceBlockQuest;
                break;
            case CATEGORY_REMOVE_BLOCK:
                BlockSystem.Instance.Events.OnRemoveBlock -= UpdateRemoveBlockQuest;
                break;
            default:
                break;
        }

        Debug.Log($"Quest Complete! ::: {quest.Data.Name}, {quest.CurrentProgress}/{quest.MaxProgress}");
        activeQuest = null;
    }
    
    private void UpdatePlaceBlockQuest(BlockSystem.BlockEvent blockEvent)
    {
        int prefabIndex = int.Parse(activeQuest.Data.Parameter.Split("_")[0]);
        if (blockEvent.Block.Data.PrefabIndex != prefabIndex) return;
        
        activeQuest.CurrentProgress++;
        Debug.Log($"Quest Update! ::: {activeQuest.Data.Name}, {activeQuest.CurrentProgress}/{activeQuest.MaxProgress}");
        if(activeQuest.CurrentProgress >= activeQuest.MaxProgress) CompleteQuest(activeQuest);
    }
    
    private void UpdateRemoveBlockQuest(BlockSystem.BlockEvent blockEvent)
    {
        int prefabIndex = int.Parse(activeQuest.Data.Parameter.Split("_")[0]);
        if (blockEvent.Block.Data.PrefabIndex != prefabIndex) return;
        
        activeQuest.CurrentProgress++;
        Debug.Log($"Quest Update! ::: {activeQuest.Data.Name}, {activeQuest.CurrentProgress}/{activeQuest.MaxProgress}");
        if(activeQuest.CurrentProgress >= activeQuest.MaxProgress) CompleteQuest(activeQuest);
    }
    

    private QuestData[] GetQuestOrNull()
    {
        QuestData[] quests = null;
        
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = true,
            Mode = CsvMode.NoEscape,
            BadDataFound = null
        };

        using (StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "QuestTable.tsv")))
        using (CsvReader cr = new CsvReader(sr,csvConfig))
        {
            var records = cr.GetRecords<QuestData>();
            quests = records.ToArray();
        }
        
        return quests;
    }
}
