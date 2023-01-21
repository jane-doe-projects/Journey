using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIndicator : MonoBehaviour
{
    GameObject indicator;
    Transform indicatorParent;
    QuestIndicationType currentType;
    float rotationSpeed = 80;
    Quaternion currentRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        currentType = QuestIndicationType.Available;
        indicatorParent = GetComponent<QuestGiver>().indicatorParent;
        indicator = Instantiate(GameManager.Instance.globalPrefabs.questAvailable, indicatorParent);
    }

    void Update()
    {
        indicator.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
        currentRotation = indicator.transform.rotation;
    }


    public void Indicate(QuestIndicationType type)
    {
        if (currentType == type)
            return;
        switch(type)
        {
            case QuestIndicationType.Available:
                ReplaceIndicator(GameManager.Instance.globalPrefabs.questAvailable);
                break;
            case QuestIndicationType.InProgress:
                ReplaceIndicator(GameManager.Instance.globalPrefabs.questInProgress);
                break;
            case QuestIndicationType.Deliverable:
                ReplaceIndicator(GameManager.Instance.globalPrefabs.questDeliverable);
                break;

            default: //Completed
                Destroy(indicator);
                this.enabled = false;
                break;
        }
        currentType = type;
    }


    void ReplaceIndicator(GameObject newIndicator)
    {   
        Destroy(indicator);
        indicator = Instantiate(newIndicator, indicatorParent.position, currentRotation, indicatorParent);
    }
}

public enum QuestIndicationType
{
    Available,
    InProgress,
    Deliverable,
    Completed
}
