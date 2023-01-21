using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIState uiState;
    public GameState gameState;
    public GlobalIcons globalIcons;
    public GlobalPrefabs globalPrefabs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        uiState = GetComponent<UIState>();
        gameState = GetComponent<GameState>();
        globalIcons = GetComponent<GlobalIcons>();
        globalPrefabs = GetComponent<GlobalPrefabs>();
    }
}
