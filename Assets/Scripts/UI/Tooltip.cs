using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;

    // TODO need to add check for cursor position, if tooltip doesnt fit on screen, swap anchor side

    public TooltipType type;
    public TooltipInfo info;
    public GameObject tooltip;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    public Image ico1;
    public Image ico2;
    public Image ico3;
    public TextMeshProUGUI info1;
    public TextMeshProUGUI info2;
    public TextMeshProUGUI info3;


    public GameObject infoArea;
    public Image[] iconList;
    public TextMeshProUGUI[] infoList;


    private void Awake()
    {
        Instance = this;
        ico1 = item1.GetComponentInChildren<Image>();
        ico2 = item2.GetComponentInChildren<Image>();
        ico3 = item3.GetComponentInChildren<Image>();
        info1 = item1.GetComponentInChildren<TextMeshProUGUI>();
        info2 = item2.GetComponentInChildren<TextMeshProUGUI>();
        info3 = item3.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        HideTooltip();
        tooltip.GetComponent<Image>().raycastTarget = false;

        iconList = infoArea.GetComponentsInChildren<Image>();
        infoList = infoArea.GetComponentsInChildren<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ShowTooltip(TooltipInfo info, TooltipType type)
    {
        // set tooltip content
        title.text = info.titel.ToString();
        description.text = info.description.ToString();

        if (info.hasRequirements)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < info.infoItemList.Count)
                {
                    iconList[i].enabled = true;
                    infoList[i].enabled = true;
                    iconList[i].sprite = info.infoItemList[i].icon;
                    infoList[i].text = info.infoItemList[i].info.ToString();

                    bool hasReq = PlayerController.Instance.playerInventoryController.genInventory.HasItemAndAmount(info.infoItemList[i].infoItem, info.infoItemList[i].info);
                    if (!hasReq)
                        infoList[i].color = Color.red;
                    else
                        infoList[i].color = Color.white;
                } else
                {
                    // hide rest of requirements items
                    iconList[i].enabled = false;
                    infoList[i].enabled = false;
                }
            }

        } else if (info.hasBenefits)
        {   /* TODO
            for (int i = 0; i < 3; i++)
            {
                if (i < info.benefitItemList.Count)
                {
                    iconList[i].enabled = true;
                    infoList[i].enabled = true;
                    iconList[i].sprite = info.benefitItemList[i].icon;
                    infoList[i].text = info.benefitItemList[i].info.ToString();
                    infoList[i].color = Color.white;
                    
                }
                else
                {
                    // hide rest of requirements items
                    iconList[i].enabled = false;
                    infoList[i].enabled = false;
                }
            } */

        } else
        {
            // deactivate additional info for now
            foreach (Image img in iconList)
            {
                img.enabled = false;
            }
            foreach (TextMeshProUGUI infoText in infoList)
            {
                infoText.enabled = false;
            }
        }
        

        // show tooltip
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public TooltipInfo GetTooltipInfo(GameObject hoverObj, TooltipType type)
    {
        TooltipInfo info = new TooltipInfo();
        
        if (type == TooltipType.Crafting)
        {
            CraftingUISlot slot = hoverObj.GetComponent<CraftingUISlot>();
            info.titel = slot.craftingSlot.item.label;
            info.description = slot.craftingSlot.item.description;
            info.hasRequirements = true;
            List<CraftingRequirements> requirements = slot.craftingSlot.item.GetRequirements();
            info.infoItemList = new List<InfoItem>();
            if (requirements.Count > 0)
            {
                for (int i = 0; i < requirements.Count; i++)
                {
                    InfoItem item = new InfoItem();
                    item.icon = requirements[i].item.icon;
                    item.info = requirements[i].requiredAmount;
                    item.infoItem = requirements[i].item;
                    info.infoItemList.Add(item);
                }
            }


        } else if (type == TooltipType.Item)
        {
            info.hasRequirements = false;
            InventoryUISlot slot = hoverObj.GetComponent<InventoryUISlot>();
            info.hasBenefits = slot.inventorySlot.item.hasBenefits;
            info.titel = slot.inventorySlot.item.label;
            info.description = slot.inventorySlot.item.description;

            if (info.hasBenefits)
                info.benefitItemList = slot.inventorySlot.item.GetBenefits();

        } else if (type == TooltipType.ActionBar)
        {
            info.hasRequirements = false;
            info.titel = OnActionBar.slot.actionItem.label;
            info.description = OnActionBar.slot.actionItem.description;
            info.hasBenefits = OnActionBar.slot.actionItem.hasBenefits;
            if (info.hasBenefits)
                info.benefitItemList = OnActionBar.slot.actionItem.GetBenefits();
        } 
          
        return info;
    }

}

public enum TooltipType
{
    Crafting,
    Item,
    ActionBar,
}

public class TooltipInfo
{
    public string titel;
    public string description;
    public bool hasRequirements;
    public List<InfoItem> infoItemList;

    public bool hasBenefits;
    public List<InfoItem> benefitItemList;
    /*
    public Sprite icon;
    public int amount;
    */
}

public class InfoItem
{
    public Sprite icon;
    public int info;

    public ItemObject infoItem;
} 

