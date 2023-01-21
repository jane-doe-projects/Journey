using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOfDayProgress : MonoBehaviour
{
    Slider dayProgress;

    private void Awake()
    {
        dayProgress = GetComponent<Slider>();
        dayProgress.minValue = 0;
        dayProgress.maxValue = 24;
    }

    void Update()
    {
        UpdateProgress(GameManager.Instance.gameState.timeOfDay);
    }

    void UpdateProgress(float value)
    {
        dayProgress.value = value;
    }
}
