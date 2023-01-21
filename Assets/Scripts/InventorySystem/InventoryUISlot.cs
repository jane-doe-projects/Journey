using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    public Image icon;
    public int amount;
    public TextMeshProUGUI amountText;
    public InventorySlot inventorySlot;
    public InventoryObject targetInventory;

    public void AddItem (InventorySlot newItem)
    {
        inventorySlot = newItem;
        if (!(inventorySlot.item == null))
            icon.sprite = inventorySlot.item.icon;
        icon.enabled = true;
        amount = inventorySlot.amount;
        amountText.text = amount.ToString();
        amountText.enabled = true;
        icon.color = Color.white;
    }

    public void ClearSlot()
    {
        inventorySlot = null;
        icon.sprite = null;
        icon.enabled = false;
        amountText.enabled = false;
    }

}
