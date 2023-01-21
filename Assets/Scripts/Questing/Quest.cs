using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
[System.Serializable]
public class Quest : ScriptableObject
{
    public bool isActive;
    public bool completed;
    public string title;
    public string description;
    public QuestGiver questGiver;

    public QuestGoal goal;
    //public List<QuestGoal> goalList; // TODO multiple goals for one quest
    public QuestReward reward;
    //public List<QuestReward> rewardList; // TODO multiple rewards for one quest

}
