using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionControl : MonoBehaviour
{
    public WindZone mainWindZone;
    public ParticleSystem windParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        WeatherControl.windDirectionDelegate += ChangeWindDirectionEffect;
        //windParticleSystem.transform.rotation = mainWindZone.transform.rotation;
    }

    public void ChangeWindDirectionEffect()
    {
        // TODO SMOOTH ROTATION SO THE PARTICLES DONT SKIP/JUMP
        windParticleSystem.transform.rotation = mainWindZone.transform.rotation;
    }
}
