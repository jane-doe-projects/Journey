using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    public List<ActionBarSlot> actionBarSlots;
    public GameObject slotsParent;

    // Start is called before the first frame update
    void Start()
    {
        if (slotsParent == null)
        {
            Debug.LogError("ActionBar Slots Parent is not set.");
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        actionBarSlots = new List<ActionBarSlot>(transform.GetComponentsInChildren<ActionBarSlot>());
    }

    public void UpdateActionBarWithItem(ItemObject item)
    {
        foreach (ActionBarSlot slot in actionBarSlots)
        {
            if (slot.actionItem != null)
            {
                if (slot.actionItem == item)
                {
                    slot.UpdateSlot();
                }
            }
        }
    }

    public void DeactivateSlotForItem(ItemObject item)
    {
        foreach (ActionBarSlot slot in actionBarSlots)
        {
            if (slot.actionItem == item)
            {
                slot.Unhighlight();
            }
        }
    }

    public void ActivateSlotForItem(ItemObject item)
    {
        if (!(item.type == ItemType.Tool) && !(item.type == ItemType.Equipment))
            return;
        foreach (ActionBarSlot slot in actionBarSlots)
        {
            if (slot.actionItem == item)
            {
                slot.Highlight();
            }
        }
    }

    public void UpdateActionBarStates()
    {
        //ToolObject current = PlayerController.Instance.gearControl.GetCurrentTool();
        foreach (ActionBarSlot slot in actionBarSlots)
        {
            if (slot.actionItem != null)
            {
                /*
                if (slot.actionItem.type == ItemType.Tool)
                {

                    if (current == slot.actionItem)
                        slot.Highlight();
                    else
                        slot.Unhighlight();
                } */

                if (slot.actionItem.type == ItemType.Equipment || slot.actionItem.type == ItemType.Tool)
                {
                    ItemObject currItem = PlayerController.Instance.gearControl.GetCurrentItemForGearSlot(slot.actionItem);
                    if (currItem == slot.actionItem)
                        slot.Highlight();
                    else
                        slot.Unhighlight();
                } else
                {
                    slot.Unhighlight();
                }
            }
        }
    }

    public bool ItemIsLastOnBar(ItemObject item)
    {
        int count = 0;
        foreach (ActionBarSlot slot in actionBarSlots)
        {
            if (slot.actionItem != null)
            {
                if (slot.actionItem == item)
                    count++;
            }
        }

        if (count <= 1)
            return true;
        return false;
    }

    public bool IsGearOnBar(ItemObject item)
    {
        foreach (ActionBarSlot slot in actionBarSlots)
        {
            if (slot.actionItem == item)
                return true;
        }

        return false;
    }
}
