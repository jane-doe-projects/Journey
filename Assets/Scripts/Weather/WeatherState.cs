using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherState : MonoBehaviour
{
    public WeatherStateType current;
    public WeatherCondition currentCondition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum WeatherStateType
{
    Sunny,
    Rain,
    Snow,
    Cloudy,
    RainStorm,
    SnowStorm,
    WindStorm,
    HeatWave
}
