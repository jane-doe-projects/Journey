using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ActionBarSlot : MonoBehaviour
{
    public ItemObject actionItem;
    public Image icon;
    public int itemCount;

    [SerializeField] bool a1, a2, a3, a4, a5;
    [SerializeField] KeyCode actionKeybind;
    [SerializeField] TextMeshProUGUI actionButtonText;
    [SerializeField] TextMeshProUGUI itemCountText;
    
    [SerializeField] Image background;
    [SerializeField] Image pressOverlay;
    Color defaultBackColor;
    public Color onPressColor;
    public Color highlightColor;

    public MouseItem mouseItem;
    private Transform canvasParent;

    void Start()
    {
        pressOverlay.enabled = false;
        defaultBackColor = background.color;
        canvasParent = GameObject.Find("Canvas").transform;
        ClearActionItem();
        actionKeybind = SetBind();
        actionButtonText.text = StringBind(actionKeybind.ToString());

        InitFunctionality();
    }

    void Update()
    {
        if (Input.GetKeyDown(actionKeybind))
        {
            ExecuteAction();
        }
    }

    void InitFunctionality()
    {
        mouseItem = new MouseItem();
        GameObject eventGameObject = this.gameObject; // need this so an index out of bounds isnt thrown on call
        AddEvent(eventGameObject, EventTriggerType.PointerEnter, (eventData) => { OnEnter(eventGameObject); });
        AddEvent(eventGameObject, EventTriggerType.PointerExit, delegate { OnExit(eventGameObject); });
        AddEvent(eventGameObject, EventTriggerType.BeginDrag, delegate { OnDragStart(eventGameObject); });
        AddEvent(eventGameObject, EventTriggerType.EndDrag, delegate { OnDragEnd(eventGameObject); });
        AddEvent(eventGameObject, EventTriggerType.Drag, delegate { OnDrag(eventGameObject); });
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);

        // I JUST HAVE THIS HERE FOR FUTURE REFERENCE
        //eventTriggerEnter.callback.AddListener((eventData) => { OnEnter(obj); });
    }

    private void ClearActionItem(bool unsetItem = true)
    {
        if (actionItem)
        {            
            if (actionItem.type == ItemType.Tool || actionItem.type == ItemType.Equipment)
            {
                ItemObject currentItem = PlayerController.Instance.gearControl.GetCurrentItemForGearSlot(actionItem);
                if (currentItem == actionItem)
                {
                    if (PlayerController.Instance.actionBarController.ItemIsLastOnBar(actionItem))
                    {
                        if (unsetItem) // to not unequip tool or equip on action bar slot change
                            actionItem.Use();
                    }

                }
            }
        }

        actionItem = null;
        if (actionItem == null)
            itemCountText.enabled = false;
        icon.enabled = false;
        itemCount = 0;
        background.color = defaultBackColor;
    }

    public void ExecuteAction()
    {
        // check what kind of item / action type is in that slot
        if (actionItem != null)
        {
            bool inUse = actionItem.Use();
            if (inUse)
            {
                // highlight actionbar slot
                PlayerController.Instance.actionBarController.ActivateSlotForItem(actionItem);

            } else
            {
                // visual on press
                StartCoroutine("Pressed");
                // unhighlight action slot
                //background.color = defaultBackColor;
            }
        }
    }

    KeyCode SetBind()
    {
        if (a1)
            return PlayerController.Instance.controls.action1;
        else if (a2)
            return PlayerController.Instance.controls.action2;
        else if (a3)
            return PlayerController.Instance.controls.action3;
        else if (a4)
            return PlayerController.Instance.controls.action4;

        return PlayerController.Instance.controls.action5;
    }

    string StringBind(string bind)
    {
        string substring = "Alpha";
        bind = bind.Replace(substring, "");

        return bind;
    }

    IEnumerator Pressed()
    {
        pressOverlay.enabled = true;   
        yield return new WaitForSeconds(0.25f);
        pressOverlay.enabled = false;
    }

    public void SetActionSlotItem(ItemObject item)
    {
        /* TODO 
        if (actionItem != null)
        {
            if (actionItem.type == ItemType.Tool || actionItem.type == ItemType.Equipment)
            {
                if (PlayerController.Instance.actionBarController.ItemIsLastOnBar(actionItem) && PlayerController.Instance.gearControl.IsGeared(actionItem))
                {
                    actionItem.Use();
                }
            }
        } */

        ItemObject tempItem = actionItem;

        actionItem = item;
        itemCountText.enabled = true;
        itemCount = PlayerController.Instance.playerInventoryController.genInventory.GetCurrentAmount(actionItem);
        itemCountText.text = itemCount.ToString();
        icon.enabled = true;
        if (item.type == ItemType.Tool || item.type == ItemType.Equipment)
            itemCountText.enabled = false;
        icon.sprite = actionItem.icon;

        if (tempItem != null && PlayerController.Instance.gearControl.IsGeared(tempItem) && !PlayerController.Instance.actionBarController.IsGearOnBar(tempItem))
        {
            tempItem.Use();
        } 
        PlayerController.Instance.actionBarController.UpdateActionBarStates();
    }

    public void UpdateSlot()
    {
        if (actionItem != null)
        {
            itemCount = PlayerController.Instance.playerInventoryController.genInventory.GetCurrentAmount(actionItem);
            if (itemCount == 0)
            {
                ClearActionItem();
            } else
            {
                itemCountText.text = itemCount.ToString();
            }
        }
    }

    public void OnEnter(GameObject obj)
    {
        OnActionBar.slot = this;
        if (actionItem != null)
        {
            TooltipInfo info = Tooltip.Instance.GetTooltipInfo(obj, TooltipType.ActionBar);
            Tooltip.Instance.ShowTooltip(info, TooltipType.ActionBar);
        }
    }

    public void OnExit(GameObject obj)
    {
        Tooltip.Instance.HideTooltip();

        OnActionBar.slot = null;
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        if (actionItem != null)
        {
            GameObject mouseObject = new GameObject("DragItem");
            RectTransform rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            mouseObject.transform.SetParent(canvasParent);

            Image dragImg = mouseObject.AddComponent<Image>();
            dragImg.sprite = actionItem.icon;
            dragImg.raycastTarget = false;

            mouseItem.obj = mouseObject;
            mouseItem.item = actionItem;
        }
    }

    public void OnDragEnd(GameObject obj)
    {
        if (OnActionBar.slot == null)
        {
            ClearActionItem();
        } else if (OnActionBar.slot.transform != obj.transform && OnActionBar.slot != null && actionItem != null)
        {
            ItemObject temp = actionItem;
            if (OnActionBar.slot.actionItem == null)
                ClearActionItem(false);
            else
                SetActionSlotItem(OnActionBar.slot.actionItem);
            OnActionBar.slot.SetActionSlotItem(temp);

        }

        PlayerController.Instance.actionBarController.UpdateActionBarStates();

        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public void OnDrag(GameObject obj)
    {
        // update position of mouse object
        if (mouseItem.obj != null)
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void Unhighlight()
    {
        background.color = defaultBackColor;
    }

    public void Highlight()
    {
        background.color = highlightColor;
    }
}

