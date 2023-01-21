using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public string interactionText;
    public string interactionName;
    public float interactionRange = 2f;

    public InteractionType interactionType;

    public virtual bool IsInteractable()
    {
        return false;
    }

    public virtual bool IsProspectable()
    {
        return false;
    }

    public virtual bool Interact()
    {
        InteractionEffect effect = GetComponent<InteractionEffect>();

        if (effect != null)
            effect.PlayEffect();

        return false;
    }

}

public enum InteractionType
{
    Loot,
    Prospecting,
    Quest,
    Activation,
    Use
}
