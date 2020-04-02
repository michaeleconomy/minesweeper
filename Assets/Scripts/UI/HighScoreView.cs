using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreView : MonoBehaviour {
    public Text nameText, scoreText;

    public void Initialize(HighScore highScore) {
        nameText.text = highScore.difficulty.name;
        scoreText.text = string.Format(CultureInfo.InvariantCulture,
            "{0}:{1:00.00}", (int)highScore.seconds / 60, highScore.seconds % 60);
    }
}


