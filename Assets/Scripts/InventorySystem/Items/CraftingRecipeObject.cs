using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe Object", menuName = "Inventory System/Items/Crafting Recipe")]
public class CraftingRecipeObject : ItemObject
{
    public List<CraftingRequirements> requiredForCrafting;
    //public ItemObject requiredForCrafting;

    private void Awake()
    {
        type = ItemType.Recipe;
        //requiredForCrafting = new List<CraftingRequirements>();
    }

    public override List<CraftingRequirements> GetRequirements()
    {
        return requiredForCrafting;
    }
}

[System.Serializable]
public class CraftingRequirements
{
    public ItemObject item;
    public int requiredAmount;
}
