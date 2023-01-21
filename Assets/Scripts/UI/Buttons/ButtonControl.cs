using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonControl : MonoBehaviour
{
    public string description;
    public TextMeshProUGUI text;
    public ButtonType buttonType;    

    // Start is called before the first frame update
    void Start()
    {
        string fullDescription = description + GetButtonBindText();
        text.text = fullDescription;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetButtonBindText()
    {
        string keybindDescription = "[ ]";
        string buttonBind = "";
        if (buttonType == ButtonType.Accept)
            buttonBind = PlayerController.Instance.controls.accept.ToString();
        else if (buttonType == ButtonType.Decline)
            buttonBind = PlayerController.Instance.controls.decline.ToString();
        else
            buttonBind = "No bind";

        buttonBind = StringBind(buttonBind); // remove "alpha" in front of bind name in case it is present
        keybindDescription = " [" + buttonBind + "]";

        return keybindDescription;
    }

    string StringBind(string bind)
    {
        string substring = "Alpha";
        bind = bind.Replace(substring, "");
        // remove "alpha" in front of bind name in case it is present
        return bind;
    }

    public void SetText(string text)
    {
        description = text;
        string fullDescription = description + GetButtonBindText();
        this.text.text = fullDescription;
    }
}

public enum ButtonType
{
    Accept,
    Decline,
    Other
}
