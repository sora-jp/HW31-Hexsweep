using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public Image background;
    public TMPro.TextMeshProUGUI text;

    Difficulty diff;

    public void SetDifficulty(Difficulty diff)
    {
        this.diff = diff;
        background.color = diff.buttonColor;
        text.SetText(diff.name);
    }

    public void PlayDiff()
    {
        GameController.currentDifficulty = diff;
        SceneManager.LoadScene(1);
    }
}
