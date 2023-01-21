using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int restoreThirstValue;
    public int restoreHungerValue;
    private void Awake()
    {
        type = ItemType.Food;
    }

    public override bool Use()
    {
        // remove from inventory
        PlayerController.Instance.playerInventoryController.genInventory.RemoveItem(this, 1);
        PlayerController.Instance.actionBarController.UpdateActionBarWithItem(this);

        // apply effects to player state
        if (restoreHungerValue > 0)
            PlayerController.Instance.state.UpdateHunger(restoreHungerValue);
        if (restoreThirstValue > 0)
            PlayerController.Instance.state.UpdateThirst(restoreThirstValue);

        return false;
    }

    public override List<InfoItem> GetBenefits()
    {
        List<InfoItem> infos = new List<InfoItem>();
        if (restoreThirstValue != 0)
        {
            InfoItem infoThrist = new InfoItem();
            infoThrist.icon = GameManager.Instance.globalIcons.thirstIcon;
            infoThrist.info = restoreThirstValue;
            infoThrist.infoItem = null;
            infos.Add(infoThrist);
        }
        if (restoreHungerValue != 0)
        {
            InfoItem infoHunger = new InfoItem();
            infoHunger.icon = GameManager.Instance.globalIcons.hungerIcon;
            infoHunger.info = restoreHungerValue;
            infoHunger.infoItem = null;
            infos.Add(infoHunger);
        }
        return infos;
    }
}

[System.Serializable]
public class BenefitInfo
{
    public Benefit type;
    public int benefitValue;
}

public enum Benefit
{
    Hydration,
    Satiety
}
