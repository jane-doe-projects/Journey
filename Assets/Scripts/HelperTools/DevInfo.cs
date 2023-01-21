using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevInfo : MonoBehaviour
{
    [SerializeField] GameState gameState;
    public TextMeshProUGUI timeOfDay;
    // Start is called before the first frame update
    void Start()
    {
        timeOfDay.enabled = false;
        gameState = GameObject.FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleInfo();
        }

        UpdateInfo();
    }

    private void ToggleInfo()
    {
        timeOfDay.enabled = !timeOfDay.enabled;
    }

    private void UpdateInfo()
    {
        timeOfDay.text = "Time of Day: " + gameState.timeOfDay.ToString("F2");
    }
}
