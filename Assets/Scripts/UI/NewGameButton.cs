using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour {
    public Text nameText, descText;

    private Difficulty difficulty;

    public void Initialize(Difficulty difficulty) {
        this.difficulty = difficulty;
        nameText.text = difficulty.name;
        descText.text = difficulty.size.x + "x" + difficulty.size.y + "  bombs: " + difficulty.bombs;
    }

    public void Click() {
        GetComponentInParent<NewGameView>().gameObject.SetActive(false);
        GameManager.instance.NewGame(difficulty);
    }
}


