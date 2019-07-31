using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event System.Action OnGameOver;
    public static event System.Action OnWin;

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
        Debug.Log("u ded");
        OnGameOver?.Invoke();
    }

    public void PlayerWinYeet()
    {
        OnWin?.Invoke();
    }
}
