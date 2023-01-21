using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craft Object", menuName = "Inventory System/Items/Craft")]
public class CraftObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Craft;
        stackable = true;
    }
}
