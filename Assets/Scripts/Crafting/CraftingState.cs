using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingState : MonoBehaviour
{
    public bool isOnMouse;
    public GameObject onMouseObj;
    public CraftingRecipeObject craftObj;
    public List<CraftingRequirements> reqList;
    public GameObject childVisual;
    public GameObject playerIndicator;
    private CircleIndicator indicator;

    public float craftingRange;

    private void Awake()
    {
        craftingRange = 3;
        indicator = playerIndicator.GetComponent<CircleIndicator>();
        indicator.radius = craftingRange;
        ShowIndicator(false);
    }

    void Update()
    {
        if (isOnMouse)
        {
            ShowIndicator(true);
            // if in placeable range color range in white, else red
            float playerToObjDist = Vector3.Distance(PlayerController.Instance.transform.position, onMouseObj.transform.position);
            if (playerToObjDist <= craftingRange)
                indicator.SetOutOfRange(false);
            else
                indicator.SetOutOfRange(true);
        } else
        {
            ShowIndicator(false);
        }


        if (isOnMouse && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1)))
        {
            // remove crafting item from mouse
            ResetCraftingObjectOnMouse();
        }

        if (isOnMouse && Input.GetKeyDown(KeyCode.Mouse0))
        {
            //List<CraftingRequirements> req = CraftMaterialsAvailable();
            if (reqList != null)
            {
                PlaceCraftingObject(reqList);
            } else
            {
                Debug.Log("Dude, get some mats.");
            }
        }
    }

    public void SetCraftObjectOnMouse(GameObject obj)
    {
        // remove current object if there is one and replace it with new object
        if (isOnMouse)
            ResetCraftingObjectOnMouse();

        isOnMouse = true;
        onMouseObj = obj;
        craftObj = onMouseObj.GetComponent<Placeable>().craftObj;
        reqList = CraftMaterialsAvailable();
        childVisual = onMouseObj.transform.GetChild(0).gameObject;

        // run blinking routine if craft materials are not available
        if (reqList == null)
            InvokeRepeating("BlinkRoutine", 0.5f, 0.5f);

        // activate range visual


    }

    public void ResetCraftingObjectOnMouse()
    {
        Destroy(onMouseObj);
        craftObj = null;
        reqList = null;
        isOnMouse = false;
        childVisual = null;
        CancelInvoke();
    }

    public void PlaceCraftingObject(List<CraftingRequirements> req)
    {
        // place object (by enabling placing routine and reset on mouse values
        Placeable placeObj = onMouseObj.GetComponent<Placeable>();

        // if in placeable range place it
        float playerToObjDist = Vector3.Distance(PlayerController.Instance.transform.position, placeObj.transform.position);
        //Debug.Log(playerToObjDist);
        if (playerToObjDist <= craftingRange)
        {
            // remove required craft items from inventory
            foreach (CraftingRequirements reqItem in req)
            {
                PlayerController.Instance.playerInventoryController.genInventory.RemoveItem(reqItem.item, reqItem.requiredAmount);
            }

            placeObj.isPlaced = true; // this may be fully unnecessary - i set it anyway so fires wont jump to mouse pointer EVER
            placeObj.enabled = false;

            onMouseObj = null;
            craftObj = null;
            reqList = null;
            childVisual = null;
            isOnMouse = false;
        } else
        {
            Debug.Log("Too far bro");
        }
    }

    public List<CraftingRequirements> CraftMaterialsAvailable()
    {
        foreach (CraftingRequirements req in craftObj.requiredForCrafting)
        {
            int amount = PlayerController.Instance.playerInventoryController.genInventory.GetCurrentAmount(req.item);
            if (amount < req.requiredAmount)
                return null;
        }
        // returns the requirements list if all items are available
        return craftObj.requiredForCrafting;
    }

    public void BlinkRoutine()
    {
        // switch visual
        childVisual.SetActive(!childVisual.activeSelf);
        /*
        foreach (Transform child in onMouseObj.transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        } */

        // update - check if materials are available
        reqList = CraftMaterialsAvailable();

        // stop routine if materials now available
        if (reqList != null)
            CancelInvoke();
    }

    private void ShowIndicator(bool show)
    {
        if (show)
        {
            playerIndicator.SetActive(true);
        } else
        {
            playerIndicator.SetActive(false);
        }
    }

}
