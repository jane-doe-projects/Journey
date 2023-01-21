using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int inventorySpace;
    public InventorySlot[] Container;
    public int emptySlots;
    public List<int> emptySlotPositions;

    public void InitInventorySlots()
    {
        Container = new InventorySlot[inventorySpace];
        FillWithNull();
        UpdateEmptySlotsValues();
    }

    public void UpdateEmptySlotsValues()
    {
        emptySlots = 0;
        emptySlotPositions.Clear();
        for (int i = 0; i < Container.Length; i++)
        {
            if (Container[i] == null)
            {
                emptySlots++;
                emptySlotPositions.Add(i);
            }
        }
    }

    public int GetEmptySlotIndex()
    {
        emptySlotPositions.Sort();
        int slotIndex = emptySlotPositions[0];
        // remove index from list and reduce free slots count
        emptySlotPositions.Remove(emptySlotPositions[0]);
        emptySlots--;
        return slotIndex;
    }

    public bool AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        
        for (int i = 0; i < Container.Length; i++)
        {
            if (Container[i] != null)
            {
                if (Container[i].item == _item)
                {
                    Container[i].AddAmount(_amount);
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                    hasItem = true;
                    break;
                }
            }
        }


        if (!hasItem)
        {
            if (emptySlots < 1)
            {
                Debug.Log("Inventory is full.");
                // drop item on ground when picked
                DropItem.DropIntoWorld(_item, _amount);

                return false;
            } else
            {
                int freeIndex = GetEmptySlotIndex();
                Container[freeIndex] = new InventorySlot(_item, _amount);
                //Debug.Log("Added: " + Container[freeIndex].item.name + " to Inventory.");
                //Container.Add(new InventorySlot(_item, _amount));
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }

        }

        if (_item.questItem)
        {
            // update goal progress and quest giver handin state when removing items from the inventory
            QuestGoalTracker.Instance.UpdateProgress(_item);
        }

        // update craftablity visual of crafting items
        if (_item.type == ItemType.Craft)
            GameManager.Instance.uiState.craftMenuUIController.UpdateCraftability();

        // update actionbar if necessary
        if (_item.type == ItemType.Food)
            PlayerController.Instance.actionBarController.UpdateActionBarWithItem(_item);

        return true;
    }

    public bool RemoveItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Length; i++)
        {
            if (Container[i] == null)
                continue;
            if (Container[i].item == _item)
            {
                bool success = Container[i].RemoveAmount(_amount);
                if (success)
                {
                    if (Container[i].amount == 0)
                    {
                        RemoveItemAtIndex(i);
                        // update action bar

                    }

                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                    hasItem = true;
                    break;
                } else
                {
                    Debug.Log("Amount not available.");
                    return false;
                }
                
            }
        }

        if (!hasItem)
        {
            Debug.Log("Item not in inventory.");
            return false;
        }

        if (_item.questItem)
        {
            // update goal progress and quest giver handin state when removing items from the inventory
            QuestGoalTracker.Instance.UpdateProgress(_item);
        }

        // update buildability on removal of items
        if (_item.type == ItemType.Craft)
            GameManager.Instance.uiState.craftMenuUIController.UpdateCraftability();

        // update actionbar
        PlayerController.Instance.actionBarController.UpdateActionBarWithItem(_item);

        return true;
    }


    public void RemoveItemAtIndex(int index)
    {
        Container[index] = null;
        // increase empty slot count and add index of free slot to list
        emptySlots++;
        emptySlotPositions.Add(index);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public int GetCurrentAmount(ItemObject _item)
    {
        int currentAmount = 0;
        for (int i = 0; i < Container.Length; i++)
        {
            if (Container[i] != null)
            {
                if (Container[i].item == _item)
                    currentAmount = Container[i].amount;
            }
        }

        return currentAmount;
    }

    public void UpdateSlot(int updateIndex, ItemObject _item, int _amount)
    {
        InventorySlot slot = new InventorySlot(_item, _amount);
        Container[updateIndex] = slot;
        UpdateEmptySlotsValues();

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

    public void UpdateSlot(int updateIndex, bool empty)
    {
        if (empty)
        {
            Container[updateIndex] = null;
        }

        UpdateEmptySlotsValues();
    }

    public void MoveItem(InventorySlot item1, int index1, InventorySlot item2, int index2)
    {
        
        if (item1 == item2)
        {
            //Debug.Log("Same item. Dont bother.");
            return;
        }

        if (item2 == null)
        {
            //Debug.Log("Attempting to swap with empty space.");
            // fill empty slot with item1
            UpdateSlot(index2, item1.item, item1.amount);
            // empty slot with item1
            UpdateSlot(index1, true); // empty slot

        } else
        {
            //Debug.Log("Trying to swap: " + item1.item.name + " with " + item2.item.name);
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            UpdateSlot(index2, item1.item, item1.amount);
            UpdateSlot(index1, temp.item, temp.amount);
        }

        
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public int GetSlotIndex(InventorySlot slot)
    {
        int index = -1;

        for (int i = 0; i < Container.Length; i++)
        {
            if (Container[i] == slot)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public bool AddItemToSlot(ItemObject _item, int _amount, InventoryUISlot slot)
    {
        bool hasItem = HasItemAndAmount(_item, 1);
        bool success = AddItem(_item, _amount);

        if (!hasItem)
        {
            // move item to slot

        }

        return success;
    }

    public InventorySlot GetSlot(ItemObject item)
    {
        foreach (InventorySlot slot in Container)
        {
            if (slot.item == item)
                return slot;
        }

        return null;
    }

    public void FillWithNull()
    {
        for (int i = 0; i < Container.Length; i++)
        {
            Container[i] = null;
        }
    }

    public bool HasItemAndAmount(ItemObject item, int amount)
    {
        if (GetCurrentAmount(item) < amount)
            return false;
        return true;
    }

}
[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int val)
    {
        amount += val;
    }

    public bool RemoveAmount(int val)
    {
        if (val <= amount)
        {
            amount -= val;
            return true;
        }
        return false;
    }

}
