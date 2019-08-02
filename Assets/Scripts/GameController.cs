using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static Difficulty currentDifficulty = new Difficulty("Default", Color.magenta, 7, 0.125f);
    public static event System.Action OnGameOver;
    public static event System.Action OnWin;

    static GameController _instance;
    public static GameController Instance => _instance;

    public TileMap tileMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one...");
            Destroy(this);
            return;
        }
        _instance = this;
        tileMap.bombChance = currentDifficulty.bombChance;
        tileMap.mapSize = currentDifficulty.mapSize;
    }

    public void PlayerClickBombYeet()
    {
        Debug.Log("u ded");
        OnGameOver?.Invoke();
    }

    public void PlayerWinYeet()
    {
        OnWin?.Invoke();
    }
}

[System.Serializable]
public struct Difficulty
{
    public string name;
    public Color buttonColor;
    public int mapSize;
    public float bombChance;

    public Difficulty(string name, Color buttonColor, int mapSize, float bombChance)
    {
        this.name = name;
        this.buttonColor = buttonColor;
        this.mapSize = mapSize;
        this.bombChance = bombChance;
    }
}