using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Quest quest;
    public float defaultAlpha;
    public Color defaultColor;

    Image background;

    void Start()
    {
        background = GetComponent<Image>();
        defaultAlpha = background.color.a;
        defaultColor = Color.white;
        defaultColor.a = defaultAlpha;
        gameObject.SetActive(false);
    }

    public void RefreshUIProgress()
    {
        if (quest.goal.Evaluate())
            Completed();
        else
            InComplete();

        gameObject.SetActive(true);
    }

    public void Completed()
    {
        Color greenCompletion;
        ColorUtility.TryParseHtmlString("#86FF9C", out greenCompletion);
        greenCompletion.a = defaultAlpha;
        background.color = greenCompletion;
    }

    public void InComplete()
    {

        background.color = defaultColor;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
