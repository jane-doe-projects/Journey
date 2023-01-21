using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] public LightingManager lightningManager;
    [SerializeField] public float timeOfDay;
    [SerializeField] public WeatherControl weatherControl;

    // GameObject holding all placeable crafted items in the game
    public Transform craftParent;

    public Transform dropParent;

    private void Start()
    {
        if (lightningManager == null)
            lightningManager = GameObject.FindObjectOfType<LightingManager>();

        if (weatherControl == null)
            weatherControl = GameObject.FindObjectOfType<WeatherControl>();
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay = GetTimeOfDay();
    }

    private float GetTimeOfDay()
    {
        return lightningManager.GetTimeOfDay();
    }
}
