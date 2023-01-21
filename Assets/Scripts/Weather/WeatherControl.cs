using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherControl : MonoBehaviour
{
    public delegate void OnWindDirectionChangeDelegate();
    public static OnWindDirectionChangeDelegate windDirectionDelegate;

    public WindZone mainWindZone;

    public WeatherCondition rain;
    public WeatherCondition snow;
    public WeatherCondition sunny;

    public WeatherState state;


    private void Awake()
    {
        state = GetComponent<WeatherState>();
    }

    void Start()
    {
        StartWindControl();
        // todo - only to init wind effects direction through delegate call
        ChangeWindDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            ToggleRain();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleSnow();
        }
    }

    void ToggleRain()
    {
        if (!rain.parSys.isPlaying)
            rain.parSys.Play();
        else
            rain.parSys.Stop();
    }

    void ToggleSnow()
    {
        if (!snow.parSys.isPlaying)
            snow.parSys.Play();
        else
            snow.parSys.Stop();
    }

    void StartWindControl()
    {
        // trigger coroutine ever couple of hours minutes to slightly or strongly change the wind direction based on time of day and current weather state
        // also change on weather state change
    }

    void ChangeWindDirection()
    {
        windDirectionDelegate();
    }

}
