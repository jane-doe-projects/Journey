using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUpInformation : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI infoText;
    public GameObject info;
    public float hideCountdown = 3;

    IEnumerator currentCoroutine;

    private void Start()
    {
        info.SetActive(false);
    }
    public void ShowPickupInformation(ItemObject item, int quantity, InventoryObject inventory)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        info.SetActive(true);
        itemImage.sprite = item.icon;
        int newAmount = inventory.GetCurrentAmount(item);

        infoText.text = "Added " + quantity + " (" + newAmount + ") " + item.label + " to Inventory";

        currentCoroutine = HidePickupInformation();
        StartCoroutine(currentCoroutine);
    }

    public void ShowInventoryFullInformation()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        info.SetActive(true);
        infoText.text = "Inventory full";
        currentCoroutine = HidePickupInformation();
        StartCoroutine(currentCoroutine);
    }

    IEnumerator HidePickupInformation()
    {
        yield return new WaitForSeconds(hideCountdown);
        info.SetActive(false);
    }
}
