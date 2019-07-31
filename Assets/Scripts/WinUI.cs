using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    private void Start()
    {
        GameController.OnWin += OnWin;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        GameController.OnWin -= OnWin;
    }

    void OnWin()
    {
        gameObject.SetActive(true);
    }
}
