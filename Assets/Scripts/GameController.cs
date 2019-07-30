using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action OnWin;

    static GameController _instance;

    public static GameController Instance => _instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one...");
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void PlayerClickBombYeet()
    {
        OnGameOver?.Invoke();
    }

    public void PlayerWinYeet()
    {
        OnWin?.Invoke();
    }
}
