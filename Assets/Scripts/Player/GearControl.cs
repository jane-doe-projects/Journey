using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearControl : MonoBehaviour
{
    [SerializeField] ToolObject tool;
    [SerializeField] EquipmentObject feet;
    [SerializeField] EquipmentObject main;
    [SerializeField] EquipmentObject misc;

    [SerializeField] GameObject toolHolder;
    [SerializeField] GameObject currentTool;

    public bool EquipTool(ToolObject item)
    {
        if (item.type == ItemType.Tool)
        {
            if (tool)
            {
                if (tool == item)
                {
                    // unequip tool 
                    RemoveTool(tool);
                    tool = null;
                    return false;
                } else
                {
                    // unequip current tool
                    // equip new tool
                    RemoveTool(tool);
                    tool = item;
                    AddTool(item);
                    return true;
                }
            } else
            {
                // equip new tool
                tool = item;
                AddTool(item);
                return true;
            }

        }
        return false;
    }

    public bool EquipEquip(EquipmentObject item)
    {
        if (item.type == ItemType.Equipment)
        {
            if (item.equipSlot == EquipSlot.Feet)
            {
                if (feet)
                {
                    if (feet == item)
                    {
                        // unequip current feet
                        RemoveEquip(item);
                        feet = null;
                        return false;
                    } else
                    {
                        RemoveEquip(feet);
                        feet = item;
                        AddEquip(item);
                        return true;
                    }
                } else
                {
                    // equip new feet
                    feet = item;
                    AddEquip(item);
                    return true;
                }
                
            } else if (item.equipSlot == EquipSlot.Main)
            {
                if (main)
                {
                    if (main == item)
                    {
                        // unequip current main
                        RemoveEquip(item);
                        main = null;
                        return false;
                    }
                    else
                    {
                        RemoveEquip(main);
                        main = item;
                        AddEquip(item);
                        return true;
                    }
                } else
                {
                    // equip new main
                    main = item;
                    AddEquip(item);
                    return true;
                }

            } else if (item.equipSlot == EquipSlot.Misc)
            {
                if (misc)
                {
                    if (misc == item)
                    {
                        // unequip current misc
                        RemoveEquip(item);
                        misc = null;
                        return false;
                    }
                    else
                    {
                        RemoveEquip(misc);
                        misc = item;
                        AddEquip(item);
                        return true;
                    }
                } else
                {
                    // equip new misc
                    misc = item;
                    AddEquip(item);
                    return true;
                }

            }
        }
        return false;
    }

    public void ValidateGearedGear()
    {
        if (!PlayerController.Instance.actionBarController.IsGearOnBar(tool))
        {
            RemoveTool(tool);
            tool = null;
        }

        if (!PlayerController.Instance.actionBarController.IsGearOnBar(feet))
        {
            RemoveEquip(feet);
            feet = null;
        }

        if (!PlayerController.Instance.actionBarController.IsGearOnBar(main))
        {
            RemoveEquip(main);
            main = null;
        }

        if (!PlayerController.Instance.actionBarController.IsGearOnBar(misc))
        {
            RemoveEquip(misc);
            misc = null;
        }

    }

    public bool IsGeared(ItemObject item)
    {
        if (tool == item || feet == item || main == item || misc == item)
            return true;
        return false;
    }

    void RemoveTool(ItemObject replacedTool)
    {
        PlayerController.Instance.actionBarController.DeactivateSlotForItem(replacedTool);
        Destroy(currentTool);
    }

    void AddTool(ToolObject item)
    {
        currentTool = Instantiate(item.visualPrefabOnly, toolHolder.transform.position, item.visualPrefabOnly.transform.rotation, toolHolder.transform);
    }

    void RemoveEquip(ItemObject replacedEquip)
    {
        PlayerController.Instance.actionBarController.DeactivateSlotForItem(replacedEquip);
        // TODO reverse/remove visual changes
    }

    void AddEquip(EquipmentObject equip)
    {
        // TODO some visual changes
    }

    public bool HasTool()
    {
        if (tool != null)
            return true;
        return false;
    }

    public ToolObject GetTool()
    {
        return tool;
    }

    public ItemObject GetCurrentItemForGearSlot(ItemObject item)
    {
        if (item.type == ItemType.Tool)
            return tool;

        if (item.type == ItemType.Equipment)
        {
            EquipmentObject equip = (EquipmentObject)item;
            if (equip.equipSlot == EquipSlot.Feet)
                return feet;
            else if (equip.equipSlot == EquipSlot.Main)
                return main;
            else
                return misc;
        }

        return null;
    }


    public void Swing()
    {
        Animation anim = toolHolder.GetComponent<Animation>();

        // swing toolholder
        if (tool.swingType == SwingType.Swing)
            anim.Play("SwingTool");
        else if (tool.swingType == SwingType.Dig)
            anim.Play("DigTool");
        else
            anim.Play("OtherTool");
    }
}

public enum SwingType
{
    Swing,
    Dig,
    Other
}
