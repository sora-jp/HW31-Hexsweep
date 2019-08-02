using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButtonSpawner : MonoBehaviour
{
    public DifficultyButton buttonPrefab;
    public Difficulty[] diffs;

    private void Awake()
    {
        foreach (var d in diffs) SpawnButton(d);
    }

    void SpawnButton(Difficulty d)
    {
        DifficultyButton b = Instantiate(buttonPrefab, transform);
        b.SetDifficulty(d);
    }
}
