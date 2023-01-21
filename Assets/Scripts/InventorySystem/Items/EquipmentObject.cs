using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherBonus
{
    Rain,
    Cold,
    Heat
}

public enum EquipSlot
{
    Main,
    Feet,
    Misc
}

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public WeatherBonus weatherBonus;
    public bool enablesHiking;

    public EquipSlot equipSlot;

    private void Awake()
    {
        type = ItemType.Equipment;
        stackable = false;
        equipSlot = EquipSlot.Main;

    }

    public override bool Use()
    {
        bool retVal = PlayerController.Instance.gearControl.EquipEquip(this);
        return retVal;
    }
}
