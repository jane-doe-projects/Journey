using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Interactable
{
    public Quest quest;
    public bool isAccepted;
    public bool isDeliverable;
    public bool fullyCompleted;
    public QuestLog playerQuestLog;
    [TextArea] public string interactionDialog;
    [TextArea] public string progressDialog;
    [TextArea] public string completionDialog;

    public Transform indicatorParent;
    private QuestIndicator indicator;

    string currentDialog;

    private void Start()
    {
        this.gameObject.tag = "QuestGiver";

        if (indicatorParent == null)
        {
            indicatorParent = this.transform;
            Debug.LogWarning("Indicator parent wasnt properly set for " + this.name);
        }
        
        isAccepted = false;
        isDeliverable = false;
        fullyCompleted = false;
        currentDialog = interactionDialog;

        if (playerQuestLog == null)
        {
            playerQuestLog = GameObject.FindObjectOfType<QuestLog>();
        }

        indicator = gameObject.AddComponent<QuestIndicator>();
    }

    public bool AddQuestToLog()
    {
        if (isAccepted)
            return false;
        quest.questGiver = this;
        isAccepted = playerQuestLog.AddToLog(quest);
        if (!isAccepted)
            Debug.Log("Questlog is full.");
        return isAccepted;
    }

    public override bool IsInteractable()
    {
        /*
        if (fullyCompleted)
            return false;
        return !isAccepted || isDeliverable; */
        return true;
    }

    public override bool Interact()
    {
        if (!fullyCompleted)
        {
            if (isAccepted && !isDeliverable)
            {
                GameManager.Instance.uiState.dialogControl.SetDialog("progress title", progressDialog, DialogType.Text, this);
                return true;
            }

            if (isAccepted && isDeliverable)
            {
                // hand in
                GameManager.Instance.uiState.dialogControl.SetDialog("handin title", progressDialog, DialogType.HandIn, this);
                return true;
            }

            GameManager.Instance.uiState.dialogControl.SetDialog("accept quest dialog", interactionDialog, DialogType.Quest, this);
            return true;

        } else
        {
            GameManager.Instance.uiState.dialogControl.SetDialog("no title", completionDialog, DialogType.Text, this);
            // make sure dialog is waiting for input and input is not used for other things 
            // also close dialog if player goes out of range of quest giver - add new bigger sphere collider for this
        }

        return false;
    }

    public bool AcceptQuest()
    {
        if (!isAccepted)
        {
            quest.questGiver = this;
            isAccepted = playerQuestLog.AddToLog(quest);
            if (isAccepted)
            {
                indicator.Indicate(QuestIndicationType.InProgress);
                SoundControl.Instance.sfxSoundControl.QuestAccept();
                QuestGoalTracker.Instance.UpdateProgress(quest);
            }
            else
            {
                // TODO add some information that the questlog is full
                Debug.Log("Questlog is full.");
            }
        }

        return isAccepted;
    }

    public void ActivateHandInState()
    {
        if (!isDeliverable)
            SoundControl.Instance.sfxSoundControl.QuestDeliverable(); // only play once when previously not deliverable
        isDeliverable = true;
        currentDialog = completionDialog;
        indicator.Indicate(QuestIndicationType.Deliverable);

    }

    public void DeactivateHandInState()
    {
        isDeliverable = false;
        currentDialog = progressDialog;
        indicator.Indicate(QuestIndicationType.InProgress);
    }

    public void ActivateCompletedState()
    {
        quest.completed = true;
        fullyCompleted = true;
        indicator.Indicate(QuestIndicationType.Completed);
        SoundControl.Instance.sfxSoundControl.QuestCompleted();
    }

    /*
    public void UpdateDeliveryStatus()
    {
        bool isStillDeliverable = CheckGoalValidity();
        if (!isStillDeliverable)
            DeactivateHandInState();
    } */

    private bool HandInQuest()
    {
        bool valid = CheckGoalValidity();

        if (!valid)
        {
            DeactivateHandInState();
            Debug.Log("Progress text: " + currentDialog);
            return false;
        }

        // exchange items
        bool request = PlayerController.Instance.playerInventoryController.genInventory.RemoveItem(quest.goal.requiredItem, quest.goal.required);
        bool reward = PlayerController.Instance.playerInventoryController.genInventory.AddItem(quest.reward.reward, quest.reward.rewardAmount);

        if (request && reward)
        {
            // remove quest from quest log and mark as fully completed and handed in
            ActivateCompletedState();
            playerQuestLog.RemoveFromLog(quest);
            return true;
        }
        else
        {
            Debug.Log("SOMETHING WENT HORRIBLY WRONG WHEN EXCHANGING QUEST ITEMS!!!");
            return false;
        }
    }

    public bool ApplyCurrentAction()
    {
        if (!fullyCompleted)
        {
            if (isAccepted && isDeliverable)
            {
                return HandInQuest();
                
            } else 
                return AcceptQuest();
        }
        return false;
    }

    public bool CheckGoalValidity()
    {
        if (quest.goal.Evaluate())
            return true;
        return false;
    }
}
