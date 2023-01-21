using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lootable : Interactable
{
    public PickUpInformation pickupInfo;

    public ItemObject item;
    public InventoryObject targetInventory;

    public int quantity;
    bool isLooted;
    public bool relootable;
    public float respawnTimer;
    public GameObject respawnables;

    private IEnumerator respawnCoroutine;

    void Start()
    {
        isLooted = false;
        pickupInfo = GameObject.Find("GameManager").GetComponent<PickUpInformation>();
        SetTargetInventory();

    }

    public override bool Interact()
    {
        if (!isLooted && !relootable)
        {
            // add to inventory if not full
            isLooted = targetInventory.AddItem(item, quantity);
            // TODO dont add if inventory is full
            //Debug.Log("Looted " + item.label + ".");
            QuestGoalTracker.Instance.UpdateProgress(item);

            if (isLooted)
            {
                pickupInfo.ShowPickupInformation(item, quantity, targetInventory);
            }
            DisableAndDestroy();
            return isLooted;
        }
        else if (!isLooted)
        {
            isLooted = targetInventory.AddItem(item, quantity);

            if (isLooted)
            {
                pickupInfo.ShowPickupInformation(item, quantity, targetInventory);
            }
            // hide respawnables
            respawnables.SetActive(false);
            // trigger respawn
            respawnCoroutine = RespawnRespawnables();
            StartCoroutine(respawnCoroutine);
            //Debug.Log("Looted relootable " + item.label + ".");
            // remove looted objects and start timer to add them back after respawnTimer
            return isLooted;
        }

        return isLooted;
    }

    public override bool IsInteractable()
    {
        return !isLooted;
    }

    void DisableAndDestroy()
    {
        //gameObject.GetComponent<SphereCollider>().radius = 0f;
        gameObject.GetComponent<Collider>().enabled = false;
        LootEffects();
        Destroy(gameObject);
    }

    IEnumerator RespawnRespawnables()
    {
        LootEffects();
        yield return new WaitForSeconds(respawnTimer);
        ReactivateLootables();
    }

    private void ReactivateLootables()
    {
        isLooted = false;
        respawnables.SetActive(true);
    }

    private void SetTargetInventory()
    {
        targetInventory = PlayerController.Instance.playerInventoryController.genInventory;
    }

    public void MovementEffect()
    {
        // add movement effect
    }

    public void LootEffects()
    {
        MovementEffect();
        SoundControl.Instance.sfxSoundControl.PlayDefaultLoot();
    }
}

