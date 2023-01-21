using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] GameObject openChest;
    [SerializeField] GameObject closedChest;

    // Start is called before the first frame update
    void Start()
    {
        openChest.SetActive(false);
        closedChest.SetActive(true);
    }

    bool isWaitingForInput = false;
    bool triggerProtection = false;
    SphereCollider chestInRangeCollider;
    GameObject currentChestTarget;
    float chestInRangeRadius = 4;


    // Update is called once per frame
    void Update()
    {
        if (isWaitingForInput)
        {
            if (triggerProtection)
                triggerProtection = !triggerProtection;
            else
            {
                if (Input.GetKeyDown(PlayerController.Instance.controls.interact) && !triggerProtection)
                {
                    GameManager.Instance.uiState.ChangePlayerUIDisplayState();
                    CloseChest();
                } else if (Input.GetKeyDown(PlayerController.Instance.controls.inventory)) {
                    CloseChest();
                }
            }
        }
    }

    public void OpenChest()
    {
        openChest.SetActive(true);
        closedChest.SetActive(false);

        //open player ui and global chest inventory ui
        if (GameManager.Instance.uiState.playerUI.activeSelf == false)
        {
            GameManager.Instance.uiState.ChangePlayerUIDisplayState();
        }
        GameManager.Instance.uiState.chestInventoryUI.SetActive(true);

        GameManager.Instance.uiState.chestInventoryUI.SetActive(true);
        isWaitingForInput = true;
        triggerProtection = true;

        // add sphere collider to chest while it is open
        chestInRangeCollider = gameObject.AddComponent<SphereCollider>();
        chestInRangeCollider.isTrigger = true;
        chestInRangeCollider.radius = chestInRangeRadius;
    }

    public void CloseChest()
    {
        isWaitingForInput = false;
        triggerProtection = false;
        openChest.SetActive(false);
        closedChest.SetActive(true);
        Destroy(chestInRangeCollider);
        GameManager.Instance.uiState.chestInventoryUI.SetActive(false);
    }

    public override bool Interact()
    {
        OpenChest();
        return true;
    }

    public override bool IsInteractable()
    {
        if (openChest.activeSelf)
            return false;
        return true;
    }
}
