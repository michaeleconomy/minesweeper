using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogView : MonoBehaviour {
    public Text messageText;
    public Button buttonPrefab;
    public Transform buttons;
    
    private static List<string> confirmButtonsCopy = new List<string>{
        "OK",
        "Cancel"
    };

    private static DialogView Instance {
        get {
            var instances = Resources.FindObjectsOfTypeAll<DialogView>();
            return instances.First();
        }
    }

    public static void Prompt(string message, string buttonCopy = "OK", Action callback = null) {
        var instances = Resources.FindObjectsOfTypeAll<DialogView>();
        var buttonsCopy = new List<string>{
            buttonCopy
        };
        instances.First()?.ShowInternal(message, buttonsCopy, i => {
            callback?.Invoke();
        });
    }

    public static void Confirm(string message, Action confirmCallback, Action cancelCallback = null) {
        Instance?.ShowInternal(message, confirmButtonsCopy, i => {
            if (i == 0) {
                confirmCallback?.Invoke();
                return;
            }
            cancelCallback?.Invoke();
        });
    }


    public static void Show(string message, List<string> buttonsCopy, Action<int> callback = null) {
        Instance?.ShowInternal(message, buttonsCopy, callback);
    }

    private void ShowInternal(string message, List<string> buttonsCopy, Action<int> callback = null) {
        messageText.text = message;
        gameObject.SetActive(true);
        buttons.DeleteChildren();
        var i = 0;
        foreach (var buttonCopy in buttonsCopy) {
            var buttonId = i;
            i++;
            var button = Instantiate(buttonPrefab, buttons);
            var text = button.GetComponentInChildren<Text>();
            text.text = buttonCopy;
            button.onClick.AddListener(() => {
                ButtonClick(i, callback);
            });
        }
    }

    private void ButtonClick(int i, Action<int> callback) {
        gameObject.SetActive(false);
        callback?.Invoke(i);
    }
}