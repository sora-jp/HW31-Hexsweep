using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI statusText;
    public string winText;
    public string loseText;

    public CanvasGroup container;
    public Image background;

    private void Start()
    {
        GameController.OnGameOver += OnGameOver;
        GameController.OnWin += OnWin;

        container.alpha = 0;
        background.color = Color.clear;

        var canvas = GetComponentInParent<Canvas>();
        container.transform.localPosition = Vector2.down * canvas.pixelRect.height * 0.5f;
    }

    void OnDestroy()
    {
        GameController.OnGameOver -= OnGameOver;
        GameController.OnWin -= OnWin;
    }

    void OnGameOver()
    {
        Show(loseText);
    }

    void OnWin()
    {
        Show(winText);
    }

    void Show(string text)
    {
        statusText.SetText(text);
        container.AnimateAlpha(1, 0.4f);
        container.transform.AnimatePosition(Vector3.zero, 0.4f);
        background.AnimateColor(new Color(0, 0, 0, 0.75f), 0.4f);
    }
}
