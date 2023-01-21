using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherCondition : MonoBehaviour
{
    public ParticleSystem parSys;

    public bool isSnowing;
    public int windIntensity;
    public int temperatureIntensity;
    public int percipitationIntensity;

    // Start is called before the first frame update
    void Start()
    {
        parSys.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
