using UnityEngine;

[System.Serializable]
public class Controls
{
    [Header("Movement Keys")]
    public KeyCode forward, backward, rotateL, rotateR, toggleWalk, jump;
    
    [Header("UI Controls")]
    public KeyCode inventory, craftMenu;
    public KeyCode action1, action2, action3, action4, action5;

    [Header("Interaction Settings")]
    public KeyCode interact;
    public KeyCode accept, decline;

}
