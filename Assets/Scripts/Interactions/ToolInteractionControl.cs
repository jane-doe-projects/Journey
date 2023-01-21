using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInteractionControl : MonoBehaviour
{

    public void CheckInteractionTarget()
    {
        // check crosshair target
        // check if in range
        // interact if tool and item compatible - do sound else other
        //Debug.Log("Impact function here");
        GameObject target = CrosshairInteraction.Instance.GetCrosshairTarget();
        bool notInteractableWithTool = true;

        if (target != null)
            notInteractableWithTool = !InteractionMatch(target);
        // get interactable type for target if any available
        // if lootable then false
        // if prospectable and correct type == true

        if (target == null || notInteractableWithTool)
        {
            // nothing to interact with, play empty sound for current tool
            Debug.Log("hit nothing matching");
            SoundControl.Instance.sfxSoundControl.Tool(false);

        } else
        {
            // do tool specific interaction and play impact sound for current tool
            SoundControl.Instance.sfxSoundControl.Tool(true);
            Debug.Log("hit something matching");

            Interactable interactableTarget = target.GetComponent<Interactable>();
            interactableTarget.Interact();
        }
    }


    bool InteractionMatch(GameObject target)
    {
        ToolObject currentTool = PlayerController.Instance.gearControl.GetTool();
        Interactable interactableTarget = target.GetComponent<Interactable>();

        if (interactableTarget != null)
        {
            if (interactableTarget.interactionType == InteractionType.Prospecting)
            {
                Prospectable prospectableTarget = (Prospectable)interactableTarget;

                if (prospectableTarget.RequiredToolType() == currentTool.toolType)
                {
                    return true;
                }

            }
        }

        return false;
    }

}
