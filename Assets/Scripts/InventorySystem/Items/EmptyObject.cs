using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Empty")]
public class EmptyObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Empty;
        stackable = false;
    }
}
