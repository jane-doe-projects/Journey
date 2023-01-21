using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSoundControl : MonoBehaviour
{
    [Header("Loot")]
    public AudioSource defaultLoot;

    [Header("Quest Sounds")]
    public AudioSource questAccept;
    public AudioSource questDeliverable;
    public AudioSource questCompleted;

    [Header("Tools")]
    public AudioSource swingEmpty;
    public AudioSource swingImpact;
    public AudioSource digEmpty;
    public AudioSource digImpact;
    public AudioSource otherEmpty;
    public AudioSource otherImpact;

    public void PlayDefaultLoot()
    {
        defaultLoot.PlayOneShot(defaultLoot.clip);
    }

    public void QuestAccept()
    {
        questAccept.PlayOneShot(questAccept.clip);
    }

    public void QuestDeliverable()
    {
        questDeliverable.PlayOneShot(questDeliverable.clip);
    }

    public void QuestCompleted()
    {
        questCompleted.PlayOneShot(questCompleted.clip);
    }

    public void Tool(bool hit)
    {
        if (PlayerController.Instance.gearControl.GetTool().swingType == SwingType.Swing)
        {
            if (hit)
                swingImpact.PlayOneShot(swingImpact.clip);
            else
                swingEmpty.PlayOneShot(swingEmpty.clip);
        } else if (PlayerController.Instance.gearControl.GetTool().swingType == SwingType.Dig)
        {
            if (hit)
                digImpact.PlayOneShot(digImpact.clip);
            else
                digEmpty.PlayOneShot(digEmpty.clip);
        } else
        {
            if (hit)
                otherImpact.PlayOneShot(otherImpact.clip);
            else
                otherEmpty.PlayOneShot(otherEmpty.clip);
        }
    }
}
