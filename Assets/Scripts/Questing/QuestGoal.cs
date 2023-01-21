using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public string goalText;
    public ItemObject requiredItem;
    public int required;
    public int current;
    public bool completed = false;

    public bool Evaluate()
    {
        if (current >= required)
            completed = true;
        else
            completed = false;
        return completed;
    }
}


