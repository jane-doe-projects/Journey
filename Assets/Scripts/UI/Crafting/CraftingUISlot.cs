using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUISlot : MonoBehaviour
{
    public Image icon;
    public InventorySlot craftingSlot;
    public bool buildable;


    public void AddItem(InventorySlot newItem)
    {
        craftingSlot = newItem;
        icon.sprite = craftingSlot.item.icon;
        icon.enabled = true;
        icon.color = Color.white;
        UpdateBuildability(false);
    }

    public void ClearSlot()
    {
        //inventorySlot = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void UpdateBuildability(bool isBuildable)
    {
        buildable = isBuildable;
        if (buildable)
            icon.color = Color.white;
        else
            icon.color = Color.grey;
    }
}
