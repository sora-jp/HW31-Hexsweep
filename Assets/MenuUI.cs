using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject[] panels;
    List<CanvasGroup> _panels;

    private void Awake()
    {
        _panels = new List<CanvasGroup>();

        foreach (var p in panels)
        {
            var pan = p.GetComponent<CanvasGroup>();
            if (pan == null) pan = p.AddComponent<CanvasGroup>();
            _panels.Add(pan);
            pan.alpha = 0;
            SetInteractiveState(pan, false);
        }
        _panels[0].alpha = 1;
        SetInteractiveState(_panels[0], true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && 
            Input.GetKey(KeyCode.LeftShift) && 
            Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    public void ShowPanel(int index)
    {
        for (int i = 0; i < _panels.Count; i++)
        {
            _panels[i].AnimateAlpha(i == index ? 1 : 0, 0.5f);
            SetInteractiveState(_panels[i], i == index);
        }
    }

    void SetInteractiveState(CanvasGroup g, bool canInteract)
    {
        g.blocksRaycasts = canInteract;
        g.interactable = canInteract;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
