using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresView : MonoBehaviour {
    public HighScoreView scoreViewPrefab;
    public Transform scores;

    public void Show() {
        scores.DeleteChildren();
        foreach (var score in GameManager.instance.HighScores()) {
            var scoreView = Instantiate(scoreViewPrefab, scores);
            scoreView.Initialize(score);
        }
        gameObject.SetActive(true);
    }
}


