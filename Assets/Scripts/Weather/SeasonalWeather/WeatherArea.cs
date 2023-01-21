using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherArea : MonoBehaviour
{
    public List<WeatherCondition> weatherOptions;
    public BoxCollider weatherArea;

    void Start()
    {
        weatherArea = GetComponent<BoxCollider>();

        InitWeatherConditionSizes();
        // start weather loop
    }

    void Update()
    {
        
    }

    void InitWeatherConditionSizes()
    {
        foreach (WeatherCondition weather in weatherOptions) {
            //weather.parSys.shape.position = weatherArea.transform.position;
            //weather.parSys.shape.shapeType = ParticleSystemShapeType.Box;

        }
    }
}
