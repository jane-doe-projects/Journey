using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChestUIControl : MonoBehaviour
{
    // general inventory
    public InventoryObject globalInventory;
    public Transform globalInventoryParent;
    public InventoryUISlot[] globalSlots;

    private Transform canvasParent;

    public MouseItem mouseItem;

    void Start()
    {
        canvasParent = GameObject.Find("Canvas").transform;
        mouseItem = new MouseItem();
        globalInventory.onItemChangedCallback += UpdateGlobalInventory;
        globalSlots = globalInventoryParent.GetComponentsInChildren<InventoryUISlot>();

        InitUI();
    }

    void InitUI()
    {
        InitInventoryUiSlots(globalInventory, globalSlots);
        PlayerController.Instance.playerInventoryController.SetUISlotsTargetInventory(globalInventory, globalSlots);
    }

    void InitInventoryUiSlots(InventoryObject inventory, InventoryUISlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Container.Length)
            {
                if (inventory.Container[i] == null)
                {
                    slots[i].ClearSlot();
                    //Debug.Log("Clearing slot.");
                }
                else
                {
                    // weird stuff with icon / item null exception... CHECK THIS AT SOME POINT
                    if (inventory.Container[i].item == null)
                        slots[i].ClearSlot();
                    else
                        slots[i].AddItem(inventory.Container[i]);
                    //Debug.Log("Adding item to UI slot.");
                }
            }

            GameObject eventGameObject = slots[i].gameObject; // need this so an index out of bounds isnt thrown on call
            AddEvent(eventGameObject, EventTriggerType.PointerEnter, (eventData) => { OnEnter(eventGameObject); });
            AddEvent(eventGameObject, EventTriggerType.PointerExit, delegate { OnExit(eventGameObject); });
            AddEvent(eventGameObject, EventTriggerType.BeginDrag, delegate { OnDragStart(eventGameObject); });
            AddEvent(eventGameObject, EventTriggerType.EndDrag, delegate { OnDragEnd(eventGameObject); });
            AddEvent(eventGameObject, EventTriggerType.Drag, delegate { OnDrag(eventGameObject); });
        }
    }

    void UpdateGlobalInventory()
    {
        for (int i = 0; i < globalSlots.Length; i++)
        {
            if (i < globalInventory.Container.Length)
            {
                if (globalInventory.Container[i] == null)
                {
                    globalSlots[i].ClearSlot();
                }
                else
                {
                    globalSlots[i].AddItem(globalInventory.Container[i]);
                }
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
        //OnGlobalInventory.slot = obj.GetComponent<InventoryUISlot>();
        InventoryUISlot uiSlot = obj.GetComponent<InventoryUISlot>();
        OnGlobalInventory.slot = uiSlot;
        //Debug.Log("Index " + GetUISlotIndex(uiSlot, globalSlots));

        mouseItem.hoverObj = obj; // sets hover obj to ui slot
        if (uiSlot.inventorySlot != null)
        {
            mouseItem.hoverItem = uiSlot.inventorySlot.item;
            TooltipInfo info = Tooltip.Instance.GetTooltipInfo(obj, TooltipType.Item);
            Tooltip.Instance.ShowTooltip(info, TooltipType.Item);
        }
    }

    public void OnExit(GameObject obj)
    {
        Tooltip.Instance.HideTooltip();

        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;

        OnGlobalInventory.slot = null;
    }

    public void OnDragStart(GameObject obj)
    {
        if (OnGlobalInventory.slot != null)
        {
            OnGlobalInventory.source = OnGlobalInventory.slot;
            // maybe need to reset this value somewhere at some point?
        }

        InventoryUISlot uiSlot = obj.GetComponent<InventoryUISlot>();

        if (uiSlot.inventorySlot != null)
        {
            GameObject mouseObject = new GameObject("DragItem");
            RectTransform rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            mouseObject.transform.SetParent(canvasParent);

            if (uiSlot.inventorySlot != null)
            {
                Image dragImg = mouseObject.AddComponent<Image>();
                dragImg.sprite = uiSlot.icon.sprite;
                dragImg.raycastTarget = false;
            }

            mouseItem.obj = mouseObject;
            mouseItem.item = uiSlot.inventorySlot.item;
        }

        // add mouseItem to global current item
        if (mouseItem.item != null)
        {
            CurrentItem.item = mouseItem.item;
        }
    }

    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.item != null)
        {
            if (OnActionBar.slot != null)
            {
                OnActionBar.slot.SetActionSlotItem(mouseItem.item);
            } else if (OnInventory.slot != null)
            {

                // if slot is empty, relocate
                if (OnInventory.slot.inventorySlot == null)
                {
                    ItemObject relItem = mouseItem.item;
                    int relAmount = PlayerController.Instance.playerInventoryController.globalInventory.GetCurrentAmount(relItem);
                    PlayerController.Instance.playerInventoryController.RelocateToGeneral(relItem, relAmount, OnInventory.slot);

                }
                else if (OnInventory.slot.inventorySlot != null && OnGlobalInventory.source != null)
                {
                    ItemObject relItem1 = mouseItem.item;
                    int relAmount1 = PlayerController.Instance.playerInventoryController.globalInventory.GetCurrentAmount(relItem1);
                    ItemObject relItem2 = OnInventory.slot.inventorySlot.item;
                    int relAmount2 = PlayerController.Instance.playerInventoryController.genInventory.GetCurrentAmount(relItem2);

                    PlayerController.Instance.playerInventoryController.SwapBetween(relItem1, relAmount1, relItem2, relAmount2, OnInventory.slot, OnGlobalInventory.source);
                }

            }

            bool notOverUI = IsMouseOverUI();

            // swap item if hovering over other slot
            if (mouseItem.hoverObj)
            {
                InventoryUISlot uiSlot1 = obj.GetComponent<InventoryUISlot>();
                InventoryUISlot uiSlot2 = mouseItem.hoverObj.GetComponent<InventoryUISlot>();

                InventorySlot slot1 = uiSlot1.inventorySlot;
                InventorySlot slot2 = uiSlot2.inventorySlot;

                InventoryObject sourceInventory = uiSlot1.targetInventory;
                InventoryObject targetInventory = uiSlot2.targetInventory;

                if (sourceInventory != targetInventory)
                {
                    Debug.Log("Trying to swap out of different inventory types. No no.");
                    Destroy(mouseItem.obj);
                    mouseItem.item = null;
                    OnGlobalInventory.source = null;
                    return;
                }

                int index1, index2 = 0;
                index1 = targetInventory.GetSlotIndex(slot1);
                if (slot2 == null)
                {
                    //Debug.Log("Attempting to swap with empty slot.");
                    InventoryUISlot[] targetSlotList = globalSlots;
                    index2 = GetUISlotIndex(mouseItem.hoverObj.GetComponent<InventoryUISlot>(), targetSlotList);
                }
                else
                {
                    index2 = targetInventory.GetSlotIndex(slot2);
                }

                //Debug.Log("Attempting to switch: " + index1 + " with " + index2);
                targetInventory.MoveItem(slot1, index1, slot2, index2);

            }
            else if (notOverUI)
            {
                DropItemIntoWorld();
            }
        }

        // delete otherwise
        Destroy(mouseItem.obj);
        mouseItem.item = null;
        OnGlobalInventory.source = null;
    }

    public void OnDrag(GameObject obj)
    {
        // update position of mouse object
        if (mouseItem.obj != null)
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void DropItemIntoWorld()
    {
        // instantiate item with item count in front of player - drop on ground later TODO
        GameObject spawnPrefab = mouseItem.item.prefab;
        if (spawnPrefab == null)
            spawnPrefab = GameManager.Instance.globalIcons.MissingPrefabModel;
        GameObject droppedItem = Instantiate(spawnPrefab, PlayerController.Instance.itemSpawn.position, Quaternion.identity, GameManager.Instance.gameState.dropParent);
        int itemCount = PlayerController.Instance.playerInventoryController.globalInventory.GetCurrentAmount(mouseItem.item);

        // set new item infos for loot
        Lootable lootInfo = droppedItem.GetComponent<Lootable>();
        if (lootInfo)
        {
            lootInfo.quantity = itemCount;
            PlayerController.Instance.playerInventoryController.globalInventory.RemoveItem(mouseItem.item, itemCount);

            // start drop routine
            droppedItem.AddComponent<DropItem>();
        }
        else
        {
            Debug.Log("No lootable component on gameobject");
            Destroy(droppedItem);
        }
    }

    public bool IsMouseOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                return false;
        }

        return true;
    }

    public int GetUISlotIndex(InventoryUISlot slot, InventoryUISlot[] slotList)
    {
        int index = -1;

        for (int i = 0; i < slotList.Length; i++)
        {
            if (slotList[i] == slot)
            {
                index = i;
                break;
            }
        }

        return index;
    }

}

public  class OnGlobalInventory
{
    public static InventoryUISlot slot;
    public static InventoryUISlot source;
}
