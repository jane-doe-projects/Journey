using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIState : MonoBehaviour
{
    bool inventoryOpen;
    public PlayerInventoryController playerInventoryController;
    public CraftMenuUIController craftMenuUIController;

    public GameObject questLog;
    public QuestSlotParent questSlotParent;

    public GameObject freeLook;
    public GameObject followCam;

    public GameObject menuPanel;
    public GameObject playerUI;

    public DialogControl dialogControl;
    public GameObject chestInventoryUI;

    delegate void PlayerUIDelegate();
    PlayerUIDelegate playerUIDelegate;

    KeyCode playerUITrigger;

    private string originalX, originalY;
    CinemachineFreeLook cfl;

    private void Awake()
    {
        cfl = freeLook.GetComponent<CinemachineFreeLook>();

        originalX = cfl.m_XAxis.m_InputAxisName;
        originalY = cfl.m_YAxis.m_InputAxisName;
    }

    void Start()
    {
        craftMenuUIController = GetComponent<CraftMenuUIController>();

        playerUITrigger = PlayerController.Instance.controls.inventory;
        playerUI = GameObject.Find("PlayerUI");

        // Quest log stuff
        if (questSlotParent == null)
        {
            questSlotParent = GameObject.FindObjectOfType<QuestSlotParent>();
        }
        if (questLog == null)
        {
            questLog = GameObject.FindGameObjectWithTag("QuestLog");
        }
        if (dialogControl == null)
        {
            dialogControl = GameObject.FindGameObjectWithTag("Dialog").GetComponent<DialogControl>();
        }

        playerUI.SetActive(false);


        playerUIDelegate += ChangePlayerUIDisplayState;

        if (!playerUI.activeSelf)
            LockMouse();


    }

    private void Update()
    {
        if (Input.GetKeyDown(playerUITrigger))
        {
            ChangePlayerUIDisplayState();
        }
    }

    public void ChangePlayerUIDisplayState()
    {
        playerUI.SetActive(!playerUI.activeSelf);
        if (!playerUI.activeSelf)
        {
            chestInventoryUI.SetActive(false);
            LockMouse();
            Tooltip.Instance.HideTooltip();
            // reset craft item
            CrosshairInteraction.Instance.craftingState.ResetCraftingObjectOnMouse();
            CrosshairInteraction.Instance.ShowCrosshair();

            // reset onmouse values in case exit events got skipped TODO maybe needs to be done somehwere else or doublecheck on swap inventory stuff
            ResetOnMouseValues();
        }
        else
        {
            UnlockMouse();
            CrosshairInteraction.Instance.HideCrosshair();
        }
        /*
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (onInventoryStateChangeCallback != null)
            onInventoryStateChangeCallback.Invoke(); */
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // set freelook axis back to default
        cfl.m_YAxis.m_InputAxisName = originalY;
        cfl.m_XAxis.m_InputAxisName = originalX;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // stop updating freelook axis x and y
        cfl.m_YAxis.m_InputAxisName = "";
        cfl.m_YAxis.m_InputAxisValue = 0;
        cfl.m_XAxis.m_InputAxisName = "";
        cfl.m_XAxis.m_InputAxisValue = 0;
    }

    public void UpdateQuestLogUI()
    {
        questSlotParent.UpdateSlotList();
    }
    

    void ResetOnMouseValues()
    {
        OnGlobalInventory.slot = null;
        OnInventory.slot = null;
    }
}
