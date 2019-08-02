using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public string scoreFormat;
    public TMPro.TextMeshProUGUI placeText, scoreText;

    public void SetScore(Score score)
    {
        placeText.SetText(score.place.ToString());
        scoreText.SetText($"{score.score / 60:D2}:{score.score % 60:D2}");
    }
}

[System.Serializable]
public struct Score
{
    public string playerName;
    public int place;
    public int score;
    public string diffName;

    public Score(string playerName, int place, int score, string diffName)
    {
        this.playerName = playerName;
        this.place = place;
        this.score = score;
        this.diffName = diffName;
    }
}