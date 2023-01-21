using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestLog : MonoBehaviour
{
    public QuestSlotParent questSlotParent;
    // Start is called before the first frame update
    void Start()
    {
        if (questSlotParent == null)
        {
            questSlotParent = GameObject.FindObjectOfType<QuestSlotParent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
