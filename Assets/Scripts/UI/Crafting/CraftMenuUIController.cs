using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CraftMenuUIController : MonoBehaviour
{
    public GameObject slotsParent;
    public InventoryObject craftRecipeInventory;
    public CraftingUISlot[] craftSlots;

    public MouseItem mouseItem;

    private void Start()
    {
        mouseItem = new MouseItem();
        // get slots from slot parent
        craftSlots = slotsParent.GetComponentsInChildren<CraftingUISlot>();
        InitCraftingUISlots();
    }

    public void InitCraftingUISlots()
    {
        // init ui slots
        for (int i = 0; i < craftRecipeInventory.Container.Length; i++)
        {
            if (craftRecipeInventory.Container[i] == null || craftRecipeInventory.Container[i].amount == 0)
            {
                craftSlots[i].ClearSlot();
            } else
            {
                craftSlots[i].AddItem(craftRecipeInventory.Container[i]);
            }

            GameObject eventGameObject = craftSlots[i].gameObject; // need this so an index out of bounds isnt thrown on call
            AddEvent(eventGameObject, EventTriggerType.PointerEnter, (eventData) => { OnEnter(eventGameObject); });
            AddEvent(eventGameObject, EventTriggerType.PointerExit, delegate { OnExit(eventGameObject); });
            AddEvent(eventGameObject, EventTriggerType.PointerClick, (eventData) => { OnLeftClick(eventGameObject); });
        }
    }


    public void UpdateCraftability()
    {
        //Debug.Log("Update craftability.");
        foreach (CraftingUISlot slot in craftSlots)
        {
            bool currentBuildable = true;
            if (slot.craftingSlot.item != null)
            {
                // get requirements and check if buildable
                Placeable placeableCraftObj = slot.craftingSlot.item.prefab.gameObject.GetComponent<Placeable>();
                List<CraftingRequirements> req = placeableCraftObj.craftObj.requiredForCrafting;
                foreach (CraftingRequirements singleReq in req)
                {
                    int amount = PlayerController.Instance.playerInventoryController.genInventory.GetCurrentAmount(singleReq.item);
                    if (amount < singleReq.requiredAmount)
                    {
                        currentBuildable = false;
                        break;
                    }
                }
                slot.UpdateBuildability(currentBuildable);
            }
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        CraftingUISlot uiSlot = obj.GetComponent<CraftingUISlot>();

        if (uiSlot.craftingSlot.item != null)
        {
            // CONTINUE HERE
            TooltipInfo info = Tooltip.Instance.GetTooltipInfo(obj, TooltipType.Crafting);
            Tooltip.Instance.ShowTooltip(info, TooltipType.Crafting);
        }

        /*
        mouseItem.hoverObj = obj; // sets hover obj to ui slot
        if (uiSlot.inventorySlot != null) // check if it is an actual item 
            mouseItem.hoverItem = uiSlot.inventorySlot.item;
        */

    }

    public void OnExit(GameObject obj)
    {
        Tooltip.Instance.HideTooltip();
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnLeftClick(GameObject obj)
    {
        // TODO this works for both clicks now 
        // instantiate gameobject on mouse
        CraftingUISlot uiSlot = obj.GetComponent<CraftingUISlot>();
        Vector3 mousePos = Input.mousePosition;
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject craftObj = Instantiate(uiSlot.craftingSlot.item.prefab, spawnPosition, Quaternion.identity, GameManager.Instance.gameState.craftParent);
    }

    public Vector3 GetSpawnPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask hitMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask))
        {
            if (hit.collider != null)
            {
                return transform.position = hit.point;
            }
        }
        return Vector3.zero;
    }

    public class MouseItem
    {
        public GameObject obj;
        public ItemObject item;
        public ItemObject hoverItem;
        public GameObject hoverObj;
    }

}
