using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefliesControl : MonoBehaviour
{
    [SerializeField] ParticleSystem parSys;

    // TODO have a default profile for fireflies somewhere that you can fall back to
    public bool defaultSettings = true;
    [SerializeField] Gradient gradient;
    [SerializeField] bool useDefaultGradient = true;
    [SerializeField] float radius = 0;
    [SerializeField] int amount = 0;

    ParticleSystem.EmissionModule pEmission;

    // Start is called before the first frame update
    void Start()
    {
        if (!defaultSettings)
            InitFireflies();

        pEmission = parSys.emission;
        pEmission.enabled = false;

        LightingManager.dayTimeSwitchDelegate += ToggleEmission;
    }

    void InitFireflies()
    {
        if (radius > 0)
        {
            ParticleSystem.ShapeModule pShape = parSys.shape;
            pShape.radius = radius;
        }

        if (amount > 0)
        {
            pEmission.rateOverTime = amount;
        }

        if (!useDefaultGradient)
        {
            ParticleSystem.ColorOverLifetimeModule pColLife = parSys.colorOverLifetime;
            pColLife.color = gradient;
        }


    }

    void ToggleEmission()
    {
        pEmission.enabled = !pEmission.enabled;
    }
}
