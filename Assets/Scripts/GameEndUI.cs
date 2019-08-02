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
    public CanvasGroup scoreContainer;
    public Image background;

    private void Start()
    {
        GameController.OnGameOver += OnGameOver;
        GameController.OnWin += OnWin;

        container.alpha = 0;
        background.color = Color.clear;

        container.transform.position = GetPosForScreenLow();
        scoreContainer.transform.position = -GetPosForScreenLow();
    }

    public void ShowMain()
    {
        container.transform.AnimatePosition(Vector3.zero, 0.4f);
        scoreContainer.transform.AnimatePosition(-GetPosForScreenLow(), 0.4f);
    }

    public void ShowScores()
    {
        container.transform.AnimatePosition(GetPosForScreenLow(), 0.4f);
        scoreContainer.transform.AnimatePosition(Vector3.zero, 0.4f);
    }

    Vector3 GetPosForScreenLow()
    {
        var canvas = GetComponentInParent<Canvas>();
        return Vector2.down * canvas.pixelRect.height * canvas.transform.localScale.x / canvas.scaleFactor;
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
