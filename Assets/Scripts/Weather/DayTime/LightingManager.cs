using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Light directionalMoonLight;
    [SerializeField] LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    public float cycleDuration = 1;

    public delegate void DayTimeSwitchDelegate();
    public static DayTimeSwitchDelegate dayTimeSwitchDelegate;

    bool isDark;

    private void Update()
    {
        if (preset == null)
            return;
        if (Application.isPlaying)
        {
            // cast 24 second day to cycle durtion long day
            timeOfDay += Time.deltaTime / 2.5f / cycleDuration;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        } else
        {
            UpdateLighting(timeOfDay / 24f);
        }

        DayTimeStateCheck();

    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        // check if lights are set
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        } else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    public float GetTimeOfDay()
    {
        return timeOfDay;
    }

    void DayTimeStateCheck()
    {
        if (timeOfDay < 5 || timeOfDay > 21)
        {
            if (!isDark)
            {
                if (dayTimeSwitchDelegate != null)
                    dayTimeSwitchDelegate.Invoke();
                isDark = true;
            }
        }
        else
        {
            if (isDark)
            {
                if (dayTimeSwitchDelegate != null)
                    dayTimeSwitchDelegate.Invoke();
                isDark = false;
            }
        }
    }

}
