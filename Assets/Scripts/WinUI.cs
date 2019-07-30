using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public Image background;
    public CanvasGroup container;

    private void Start()
    {
        GameController.OnWin += OnGameOver;
        gameObject.SetActive(false);
        background.color = Color.clear;
        var c = GetComponentInParent<Canvas>();
        container.transform.position = c.pixelRect.height * Vector3.down * 0.5f * c.scaleFactor;
        container.alpha = 0;
    }

    private void OnDestroy()
    {
        GameController.OnWin -= OnGameOver;
    }

    void OnGameOver()
    {
        gameObject.SetActive(true);
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
