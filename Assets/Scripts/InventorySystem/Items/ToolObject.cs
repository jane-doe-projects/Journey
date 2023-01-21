using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public GameObject visualPrefabOnly;
    public SwingType swingType;
    public ToolType toolType;

    private void Awake()
    {
        type = ItemType.Tool;
        stackable = false;
    }

    public override bool Use()
    {
        bool retVal = PlayerController.Instance.gearControl.EquipTool(this);
        return retVal;
    }
}

public enum ToolType
{
    Mining,
    Chopping,
    Digging,
    Attacking
}
