using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public ScoreItem itemPrefab;
    float startTime;

    private void Start()
    {
        startTime = Time.realtimeSinceStartup;
        GameController.OnWin += LoadAndSaveScores;
        GameController.OnGameOver += LoadScores;
    }

    private void OnDestroy()
    {
        GameController.OnWin -= LoadAndSaveScores;
        GameController.OnGameOver -= LoadScores;
    }

    void LoadAndSaveScores()
    {
        List<Score> sortedScores = GetScoreList();
        sortedScores.Add(new Score("_empty", -1, (int)(Time.realtimeSinceStartup - startTime), GameController.currentDifficulty.name));
        sortedScores = sortedScores.OrderBy(t => t.score).ToList();
        LoadScores(sortedScores);
        Debug.Log("[LoadAndSaveScores NewJson]" + JsonUtility.ToJson(new ScoreList(sortedScores)));
        PlayerPrefs.SetString("scores", JsonUtility.ToJson(new ScoreList(sortedScores)));
    }

    void LoadScores() => LoadScores(null);

    void LoadScores(List<Score> scores)
    {
        if (scores == null) scores = GetScoreList();
        scores = scores.Where(t => t.diffName == GameController.currentDifficulty.name).ToList();
        for (int i = 0; i < 3; i++)
        {
            if (i >= scores.Count) break;
            var curScore = scores[i];
            curScore.place = i + 1;
            SpawnScore(curScore);
        }
    }

    List<Score> GetScoreList()
    {
        List<Score> scores = new List<Score>(0);
        if (PlayerPrefs.HasKey("scores"))
        {
            Debug.Log("[GetScoreList PP]: " + PlayerPrefs.GetString("scores"));
            scores = JsonUtility.FromJson<ScoreList>(PlayerPrefs.GetString("scores")).scores;
        }
        return scores;
    }

    private void SpawnScore(Score score)
    {
        ScoreItem item = Instantiate(itemPrefab, transform);
        item.SetScore(score);
    }
}

public struct ScoreList
{
    public List<Score> scores;

    public ScoreList(List<Score> scores)
    {
        this.scores = scores;
    }
}