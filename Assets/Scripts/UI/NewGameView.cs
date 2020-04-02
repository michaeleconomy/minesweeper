using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameView : MonoBehaviour {
    public NewGameButton buttonPrefab;
    public Transform buttons;

    private void Awake() {
        buttons.DeleteChildren();
        foreach (var difficulty in GameManager.difficulties) {
            var button = Instantiate(buttonPrefab, buttons);
            button.Initialize(difficulty);
        }
    }
}


