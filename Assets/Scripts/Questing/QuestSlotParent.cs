using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSlotParent : MonoBehaviour
{
    [SerializeField] QuestLog playerQuestLog;
    public Quest[] playerQuestList;
    public QuestSlot[] questSlots;

    private void Awake()
    {
        questSlots = GameObject.FindObjectsOfType<QuestSlot>();
    }

    private void Start()
    {
        if (playerQuestLog == null)
        {
            playerQuestLog = GameObject.Find("Player").GetComponent<QuestLog>();
            playerQuestList = new Quest[5];
            FetchQuests();
        }
    }

    public void UpdateSlotList()
    {
        FetchQuests();
        for (int i = 0; i < playerQuestList.Length; i++)
        {
            if (playerQuestList[i] != null)
                InitSlot(i, playerQuestList[i]);
            else
                DeactivateSlot(i);
        }
    }

    void InitSlot(int slotPos, Quest quest)
    {
        questSlots[slotPos].enabled = true;
        questSlots[slotPos].quest = quest;
        questSlots[slotPos].title.text = quest.title;
        string progress = quest.goal.current + "/" + quest.goal.required;
        //questSlots[slotPos].description.text = quest.description + progress;
        questSlots[slotPos].description.text = progress; // TODO just show progress without description for now

        bool completed = quest.goal.Evaluate();
        if (completed)
            questSlots[slotPos].Completed();
        else
            questSlots[slotPos].InComplete();

        questSlots[slotPos].RefreshUIProgress();        
    }

    void DeactivateSlot(int slotPos)
    {
        questSlots[slotPos].Disable();
    }

    void FetchQuests()
    {
        ClearCurrentList();
        int count = 0;
        foreach (Quest quest in playerQuestLog.questList)
        {
            playerQuestList[count] = quest;
            count++;
        }
    }

    void ClearCurrentList()
    {
        for (int i = 0; i < playerQuestList.Length; i++)
        {
            playerQuestList[i] = null;
        }
    }

}
