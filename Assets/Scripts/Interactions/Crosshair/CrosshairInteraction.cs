using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrosshairInteraction : MonoBehaviour
{
    public static CrosshairInteraction Instance;

    // crafting state
    public CraftingState craftingState;

    float interactionRange = 1.5f;
    float rayRange = Mathf.Infinity;
    public LayerMask layerMask;
    Transform player;
    Controls controls;

    // text
    public GameObject informationText;
    public GameObject informationTextAlternative;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemTextAlternative;
    string interactionButton = "[E]";

    // crosshair visuals
    public GameObject crosshairDefault;
    public GameObject crosshairHit;

    public GameObject visual;

    bool crosshairInteractionActive;

    // ray cast
    RaycastHit hit;
    Ray ray;

    private void Awake()
    {
        Instance = this;
        crosshairInteractionActive = true;
    }

    private void Start()
    {
        craftingState = GetComponent<CraftingState>();
        GameObject playerObj = GameObject.Find("Player");
        player = playerObj.transform;
        controls = playerObj.GetComponent<PlayerController>().controls;
        interactionButton = "[" + controls.interact.ToString() + "]";

        HideInteractionText();
    }

    // Update is called once per frame
    void Update()
    {
        if (crosshairInteractionActive)
        {
            ray = Camera.main.ScreenPointToRay(transform.position);

            if (Physics.Raycast(ray, out hit, rayRange, layerMask) && !GameManager.Instance.uiState.playerUI.activeSelf)
            {
                if (hit.collider != null)
                {
                    if (IsInInteractionRange(player, hit.collider.gameObject.transform))
                    {
                        GameObject target = hit.collider.gameObject;
                        Interactable interactableTarget = target.GetComponent<Interactable>();
                        if (interactableTarget != null)
                        {
                            if (interactableTarget.IsInteractable())
                            {
                                /*
                                if (Input.GetKeyDown(KeyCode.LeftShift))
                                {
                                    // show alternative interaction text
                                    Debug.Log("Show alternative interaction text.");
                                    //ShowAlternativeInteractionText(target);
                                } */
                                ShowInteractionText(target);
                            }

                            if (interactableTarget.IsProspectable())
                            {
                                ShowAlternativeText(target);
                            }


                            if (Input.GetKeyDown(controls.interact))
                            {
                                if (interactableTarget.Interact())
                                    HideInteractionText();
                            }
                        }
                    }
                }
            }
            else
            {
                HideInteractionText();
            }
        }
    }

    bool IsInInteractionRange(Transform player, Transform target)
    {
        bool inRange = false;
        float distance = Vector3.Distance(player.position, target.position);
        //Debug.Log(distance);

        Interactable interactableTarget = target.GetComponent<Interactable>();
        if (interactableTarget != null)
        {
            interactionRange = target.GetComponent<Interactable>().interactionRange;
            if (distance <= interactionRange)
                inRange = true;
        } else
        {
            Debug.Log("Weird." + target.name);
        }



        return inRange;
    }

    void HideInteractionText()
    {
        informationText.SetActive(false);
        informationTextAlternative.SetActive(false);
        crosshairHit.SetActive(false);
    }

    void ShowInteractionText(GameObject target)
    {
        Interactable interactableInfo = target.GetComponent<Interactable>();
        interactionText.text = interactionButton + " " + interactableInfo.interactionText;

        // get amount text if interactable item is lootable
        string amountText = "";
        Lootable lootableInfo = target.GetComponent<Lootable>();
        if (lootableInfo != null)
            amountText = " x" + lootableInfo.quantity.ToString();

        itemText.text = interactableInfo.interactionName + amountText;

        informationText.SetActive(true);
        informationTextAlternative.SetActive(false);
        crosshairHit.SetActive(true);
    }

    void ShowAlternativeText(GameObject target)
    {
        Interactable interactableInfo = target.GetComponent<Interactable>();
        itemTextAlternative.text = interactableInfo.interactionName;

        informationTextAlternative.SetActive(true);
        informationText.SetActive(false);
        crosshairHit.SetActive(true);
    }

    public void HideCrosshair()
    {
        // hide visual and disable interaction
        crosshairInteractionActive = false;
        visual.SetActive(false);
    }

    public void ShowCrosshair()
    {
        // show visual and enable interaction
        crosshairInteractionActive = true;
        visual.SetActive(true);
    }

    
    public GameObject GetCrosshairTarget()
    {
        if (crosshairInteractionActive)
        {
            ray = Camera.main.ScreenPointToRay(transform.position);

            if (Physics.Raycast(ray, out hit, rayRange, layerMask) && !GameManager.Instance.uiState.playerUI.activeSelf)
            {
                if (hit.collider != null)
                {
                    if (IsInInteractionRange(player, hit.collider.gameObject.transform))
                    {
                        return hit.collider.gameObject;
                    }
                }
            }
        }

        return null;
    }


    GameObject GetTargetInFront()
    {
        if (crosshairInteractionActive)
        {
            ray = Camera.main.ScreenPointToRay(transform.position);

            if (Physics.Raycast(ray, out hit, rayRange, layerMask) && !GameManager.Instance.uiState.playerUI.activeSelf)
            {
                if (hit.collider != null)
                {
                    if (IsInInteractionRange(player, hit.collider.gameObject.transform))
                    {
                        return hit.collider.gameObject;
                    }
                }
            }
        }

        return null;
    }
}
