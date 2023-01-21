using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Food,
    Equipment,
    Craft,
    Misc,
    Currency,
    Empty,
    Tool,
    Recipe
}
public abstract class ItemObject : ScriptableObject
{
    public bool stackable;
    public GameObject prefab;
    public Sprite icon;
    public ItemType type;
    public string label;
    public bool questItem;
    [TextArea(5,10)]
    public string description;
    public bool hasBenefits;

    private void Awake()
    {
        label = this.name;
    }

    public InventoryObject GetTargetInventory()
    {
        return PlayerController.Instance.playerInventoryController.genInventory;
    }

    public virtual bool Use()
    {
        return false;
    }

    public virtual List<CraftingRequirements> GetRequirements()
    {
        return null;
    }

    public virtual List<InfoItem> GetBenefits()
    {
        return null;
    }
}
