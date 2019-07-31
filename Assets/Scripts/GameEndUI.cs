using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public Image background;
    public CanvasGroup container;
    public string winText, loseText;
    public TMPro.TextMeshProUGUI statusText;
    Canvas c;

    private void Start()
    {
        GameController.OnGameOver += OnGameOver;
        GameController.OnWin += OnWin;
        gameObject.SetActive(false);
        c = GetComponentInParent<Canvas>();
        background.color = Color.clear;
        container.transform.position = c.pixelRect.height * Vector3.down * 0.5f * c.scaleFactor;
        container.alpha = 0;
    }

    private void OnDestroy()
    {
        GameController.OnGameOver -= OnGameOver;
        GameController.OnWin -= OnWin;
    }

    private void OnWin()
    {
        Show(winText);
    }

    private void OnGameOver()
    {
        Show(loseText);
    }

    void Show(string text)
    {
        gameObject.SetActive(true);
        statusText.SetText(text);
        background.AnimateColor(new Color(0, 0, 0, 0.8f));
        container.transform.AnimatePosition(Vector3.zero);
        container.AnimateAlpha(1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
