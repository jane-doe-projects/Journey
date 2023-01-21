using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float speedBuffMultiplier = 1;
    public float refreshInSeconds = 30f;
    private float cycleDuration;
    [SerializeField] public int baseUnitLossPerDay;
    private int baseUnitLossPerRefresh;
    private int refreshPerMinute;

    // on full state - set set out counter for deterioration refresh
    [SerializeField] int setOutCounter = 2;
    private int drinkSetOutCount = 0;
    private int hungerSetOutCount = 0;

    [Range(0, 100)]
    int satiety;

    [Range(0, 100)]
    int hydration;

    bool isWet;
    bool isHot;
    bool isCold;

    // environmental impact / debuffs
    int windIntensity;
    int temperatureIntensity;
    int percipitationIntensity;
    bool snowing;

    // ui elements
    public ResourceBar thirst;
    public ResourceBar hunger;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("InitRefreshValues", 2f);
        InitState();
        StartAutomatedStateChange();
    }

    // Update is called once per frame
    void Update()
    {
        //ProgressState();
    }

    public void InitState()
    {
        // set all values to default
        satiety = 50;
        hydration = 50;

        // update ui values
        hunger.UpdateValue(satiety);
        thirst.UpdateValue(hydration);
    }

    private void ProgressState()
    {
        // let state naturally deteriorate
        // deterioration or refresh speed based on environmental variables
    }

    private void InitRefreshValues()
    {
        // invoked once after the start since gamestate is not initialized yet (bad solution but leaving it like this for now)
        refreshPerMinute = (int)(60 / refreshInSeconds);
        cycleDuration = GameManager.Instance.gameState.lightningManager.cycleDuration;
        baseUnitLossPerRefresh = (int)(baseUnitLossPerDay / (cycleDuration * refreshPerMinute));
    }

    private void StartAutomatedStateChange()
    {
        InvokeRepeating("Thirst", 0f+refreshInSeconds, refreshInSeconds);
        InvokeRepeating("Hunger", 0.5f+refreshInSeconds, refreshInSeconds);
        InvokeRepeating("Warmth", 1f+refreshInSeconds, refreshInSeconds);
    }


    private void Thirst()
    {
        if (drinkSetOutCount > 0)
        {
            drinkSetOutCount--;
            return;
        }

        hydration -= baseUnitLossPerRefresh * 2;
        if (hydration < 0)
            hydration = 0;
        //Debug.Log("Thirst on " + hydration);
        // update ui
        thirst.UpdateValue(hydration);
    }

    private void Hunger()
    {
        if (hungerSetOutCount > 0)
        {
            hungerSetOutCount--;
            return;
        }

        satiety -= baseUnitLossPerRefresh;
        if (satiety < 0)
            satiety = 0;
        //Debug.Log("Satiety on " + satiety);

        // update ui
        hunger.UpdateValue(satiety);
    }

    private void Warmth()
    {

    }

    // TODO 
    // UPDATE DRINK AND FOOD STATE - IF THE HIT FULL 100 let deterioration update set out twice or more times?

    public void UpdateThirst(int value)
    {
        hydration += value;
        // if hydration is maxed out - stay hydrated to the max for a few deterioration cycles
        if (hydration == 100)
            drinkSetOutCount = setOutCounter;

        // ui update
        thirst.UpdateValue(hydration);
    }

    public void UpdateHunger(int value)
    {
        satiety += value;
        if (satiety == 100)
            hungerSetOutCount = setOutCounter;
        // ui update
        hunger.UpdateValue(satiety);
    }


}
