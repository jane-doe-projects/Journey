using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prospectable : Interactable
{
    // tool type that can prospect it
    [SerializeField] ToolType requiredToolType;
    [SerializeField] int requiredInteractionCount;

    [Header("Loot Information")]
    [SerializeField] GameObject itemDrop;
    [SerializeField] int dropCount;

    private void Awake()
    {
        interactionType = InteractionType.Prospecting;

    }

    public ToolType RequiredToolType() { return requiredToolType; }

    public override bool IsProspectable()
    {
        return true;
    }



    public override bool Interact()
    {
        requiredInteractionCount--;
        // add impact sound TODO
        InteractionEffect effect = GetComponent<InteractionEffect>();
        if (effect != null)
            effect.PlayEffect();

        if (requiredInteractionCount <= 0)
            DropLoot();

        return true;
    }

    void DropLoot()
    {
        // drop items
        SpawnItems();

        // add destruction sound
        // destroy ?
        Destroy(gameObject);
    }

    void SpawnItems()
    {
        GameObject spawnPrefab = itemDrop;
        if (spawnPrefab == null)
            spawnPrefab = GameManager.Instance.globalIcons.MissingPrefabModel;
        GameObject droppedItem = Instantiate(spawnPrefab, PlayerController.Instance.itemSpawn.position, Quaternion.identity, GameManager.Instance.gameState.dropParent);

        // set new item infos for loot
        Lootable lootInfo = droppedItem.GetComponent<Lootable>();
        if (lootInfo)
        {
            lootInfo.quantity = dropCount;
            // start drop routine
            droppedItem.AddComponent<DropItem>();
        }
        else
        {
            Debug.Log("No lootable component on gameobject");
            Destroy(droppedItem);
        }
    }

}
