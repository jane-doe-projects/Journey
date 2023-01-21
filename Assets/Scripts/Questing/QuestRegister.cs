using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRegister : MonoBehaviour
{
    public List<QuestGiver> questRegister;

    void Start()
    {
        QuestGiver[] questGivers = GameObject.FindObjectsOfType<QuestGiver>();
        foreach (QuestGiver giver in questGivers)
        {
            questRegister.Add(giver);
        }
    }
}
