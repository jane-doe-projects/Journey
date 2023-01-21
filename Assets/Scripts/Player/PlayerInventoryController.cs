using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PlayerInventoryController : MonoBehaviour
{
    public InventoryObject genInventory;
    public InventoryObject globalInventory;

    public InventoryObject craftInventory;

    [SerializeField] InventoryUI genControl;
    [SerializeField] ChestUIControl globalControl;

    public GameObject inventoryPanel;


    private void Start()
    {
        genInventory.InitInventorySlots();
        globalInventory.InitInventorySlots();
    }

    private void OnApplicationQuit()
    {
        genInventory.FillWithNull();
        genInventory.UpdateEmptySlotsValues();
        globalInventory.FillWithNull();
        globalInventory.UpdateEmptySlotsValues();
    }

    public void SetUISlotsTargetInventory(InventoryObject inv, InventoryUISlot[] uiSlots)
    {
        foreach (InventoryUISlot slot in uiSlots)
        {
            slot.targetInventory = inv;
        }
    }

    public bool RelocateToGlobal(ItemObject item, int amount, InventoryUISlot targetSlot)
    {
        /*
        bool hasSpace = globalInventory.HasItemAndAmount(item, 1);
        if (!hasSpace)
            hasSpace = globalInventory.emptySlots > 0;
        */
        /*
        if (hasSpace)
        {
            // do stuff
            genInventory.RemoveItem(item, amount);

            globalInventory.AddItemToSlot(item, amount, index);
            globalInventory.AddItem(item, amount);

            return true;
        }*/

        genInventory.RemoveItem(item, amount);
        int index = globalControl.GetUISlotIndex(targetSlot, globalControl.globalSlots);
        //globalInventory.UpdateSlot(index, item, amount);
        globalInventory.AddItem(item, amount);
        return true;

        //return false;
    }

    public bool RelocateToGeneral(ItemObject item, int amount, InventoryUISlot targetSlot)
    {
        globalInventory.RemoveItem(item, amount);
        int index = genControl.GetUISlotIndex(targetSlot, genControl.genSlots);
        //genInventory.UpdateSlot(index, item, amount);
        genInventory.AddItem(item, amount);
        return false;
    }

    public bool SwapBetween(ItemObject item1, int amount1, ItemObject item2, int amount2, InventoryUISlot targetSlotFor1, InventoryUISlot targetSlotFor2)
    {
        //Debug.Log("swap getting called");
        // check if items identical - then stack in targetfor1 inventory
        if (item1 == item2)
        {
            targetSlotFor2.targetInventory.RemoveItem(item1, amount1);
            targetSlotFor1.targetInventory.AddItem(item1, amount1);
            return true;
        }

        int index1 = -1;
        int index2 = -1;
        if (targetSlotFor1.targetInventory == genInventory)
        {
            //Debug.Log("target for first item is general");
            index1 = genControl.GetUISlotIndex(targetSlotFor1, genControl.genSlots);
            index2 = globalControl.GetUISlotIndex(targetSlotFor2, globalControl.globalSlots);
            //Debug.Log(index1 + " " + index2);
        }
        else
        {
            //Debug.Log("target for second item is general");
            index1 = globalControl.GetUISlotIndex(targetSlotFor1, globalControl.globalSlots);
            index2 = genControl.GetUISlotIndex(targetSlotFor2, genControl.genSlots);
            //Debug.Log(index1 + " " + index2);
        }

        if (index1 == -1 || index2 == -1)
            return false;


        // TODO CONTINUE HERE - properly swap items - by removing and adding at slot location and not through updating slots... its bad.
        // fix that same items get put on their stack automatically.
        //targetSlotFor1.targetInventory.RemoveItem(item1, amount1);
        //targetSlotFor2.targetInventory.RemoveItem(item2, amount2);

        /*
        targetSlotFor2.targetInventory.UpdateSlot(index2, item2, amount2);
        targetSlotFor1.targetInventory.UpdateSlot(index1, item1, amount1);
        */

        targetSlotFor2.targetInventory.RemoveItem(item1, amount1);
        targetSlotFor1.targetInventory.RemoveItem(item2, amount2);

        targetSlotFor1.targetInventory.AddItem(item1, amount1);
        targetSlotFor2.targetInventory.AddItem(item2, amount2);


        return true;
    }
}
