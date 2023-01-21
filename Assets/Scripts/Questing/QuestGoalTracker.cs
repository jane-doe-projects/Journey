using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoalTracker : MonoBehaviour
{
    public static QuestGoalTracker Instance;

    QuestLog playerQuestLog;
    public InventoryObject generalInventory;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerQuestLog = GetComponent<QuestLog>();
    }

    public void UpdateProgress(ItemObject item)
    {
        if (item.questItem)
        {

            List<Quest> requiredForList = new List<Quest>();
            foreach (Quest quest in playerQuestLog.questList)
            {
                if (quest.goal.requiredItem == item)
                    requiredForList.Add(quest);
            }

            if (requiredForList != null)
            {
                if (requiredForList.Count > 0)
                {
                    int currentCount = generalInventory.GetCurrentAmount(item);
                    foreach (Quest quest in requiredForList)
                    {
                        quest.goal.current = currentCount;
                        bool completed = quest.goal.Evaluate();
                        if (completed)
                            quest.questGiver.ActivateHandInState();
                        else
                            quest.questGiver.DeactivateHandInState();
                        GameManager.Instance.uiState.UpdateQuestLogUI();
                    }

                }
            }

        }
    }

    public void UpdateProgress(Quest quest)
    {
        // check inventory for required items of quest
        int currentCount = generalInventory.GetCurrentAmount(quest.goal.requiredItem);
        quest.goal.current = currentCount;
        bool completed = quest.goal.Evaluate();
        if (completed)
            quest.questGiver.ActivateHandInState();
        GameManager.Instance.uiState.UpdateQuestLogUI();
    }
}
