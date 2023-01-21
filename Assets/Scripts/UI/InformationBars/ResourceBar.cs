using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] public int maxVal;
    [SerializeField] int minVal;
    [SerializeField] Color fillColor;
    [SerializeField] Color backgroundColor;
    [SerializeField] Image fill;
    [SerializeField] Image background;

    // Start is called before the first frame update
    void Start()
    {
        fill.color = fillColor;
        background.color = backgroundColor;
    }

    public void UpdateValue(int current)
    {
        slider.value = current;
    }


    private void OnValidate()
    {
        fill.color = fillColor;
        background.color = backgroundColor;
    }
}
