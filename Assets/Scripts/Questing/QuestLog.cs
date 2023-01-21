using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    public List<Quest> questList;

    public bool AddToLog(Quest quest)
    {
        if (questList.Count == 5)
            return false;
        questList.Add(quest);
        UpdateQuestUI();
        return true;
    }

    public void RemoveFromLog(Quest quest)
    {
        questList.Remove(quest);
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        GameManager.Instance.uiState.UpdateQuestLogUI();
    }
}

