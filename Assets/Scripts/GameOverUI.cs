using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private void Start()
    {
        GameController.OnGameOver += OnGameOver;
        gameObject.SetActive(false);
    }

    void OnGameOver()
    {
        gameObject.SetActive(true);
    }
}
