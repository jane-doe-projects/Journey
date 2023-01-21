using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogControl : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI mainText;
    public ButtonControl acceptButton;
    public ButtonControl declineButton;
    public ButtonControl continueButton;

    public QuestGiver currentDialogTarget;
    public SphereCollider dialogInRangeCollider;
    [SerializeField] float dialogInRangeRadius = 4;


    [SerializeField] string acceptText = "Accept";
    [SerializeField] string exchangeText = "Exchange";

    bool isWaitingForInput;
    bool triggerProtection;
    KeyCode acceptKey;
    KeyCode declineKey;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        isWaitingForInput = false;
        triggerProtection = true;
        acceptKey = PlayerController.Instance.controls.accept;
        declineKey = PlayerController.Instance.controls.decline;
    }

    private void Update()
    {
        if (isWaitingForInput)
        {
            if (triggerProtection)
                triggerProtection = !triggerProtection;
            else
            {
                if (Input.GetKeyDown(declineKey) && !triggerProtection && (declineButton.gameObject.activeSelf || continueButton.gameObject.activeSelf))
                {
                    CloseDialog();
                }
                else if (Input.GetKeyDown(acceptKey) && !triggerProtection && acceptButton.gameObject.activeSelf)
                {
                    // trigger interaction
                    Debug.Log("Trigger some action.");
                    bool success = currentDialogTarget.ApplyCurrentAction();
                    if (success)
                        CloseDialog();
                    else
                    {
                        Debug.Log("Something went wrong with the interaction. It was unsuccessful.");
                    }
                }
            }
        }
    }

    public void SetDialog(string title, string mainText, DialogType type, QuestGiver target)
    {
        currentDialogTarget = target;
        ToggleButtonType(type);
        this.title.text = title;
        this.mainText.text = mainText;
        CrosshairInteraction.Instance.HideCrosshair();
        dialogPanel.SetActive(true);
        isWaitingForInput = true;
        triggerProtection = true;

        // add sphere collider to quest giver while dialog is open
        dialogInRangeCollider = currentDialogTarget.gameObject.AddComponent<SphereCollider>();
        dialogInRangeCollider.isTrigger = true;
        dialogInRangeCollider.radius = dialogInRangeRadius;
    }

    public void CloseDialog()
    {
        // remove sphere collider for dialog radius check
        Destroy(dialogInRangeCollider);

        // cancel waiting for input 
        isWaitingForInput = false;
        currentDialogTarget = null;
        CrosshairInteraction.Instance.ShowCrosshair();
        dialogPanel.SetActive(false);
        isWaitingForInput = false;
    }

    void ToggleButtonType(DialogType type)
    {
        if (type == DialogType.Text)
        {
            acceptButton.gameObject.SetActive(false);
            declineButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
        } else
        {
            acceptButton.gameObject.SetActive(true);
            declineButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);

            if (type == DialogType.HandIn)
            {
                acceptButton.SetText(exchangeText);
            } else
            {
                acceptButton.SetText(acceptText);
            }
        }
    }

}

public enum DialogType
{
    Quest,
    HandIn,
    Text
}
