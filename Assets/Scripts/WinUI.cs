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
        container.transform.position = GetComponentInParent<Canvas>().pixelRect.width * Vector3.right * 0.5f;
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
        container.transform.AnimatePosition(GetComponentInParent<Canvas>().pixelRect.size * 0.5f);
        container.AnimateAlpha(1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
