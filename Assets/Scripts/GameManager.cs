
using System;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.iOS;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static bool firstClick = false;

    public Text clockText;
    public Square squarePrefab;
    public Transform squaresParent;
    public RectTransform topBar;
    public GameObject newGameButton, highScoresButton;

    private readonly Dictionary<Vector2Int, Square> squares = new Dictionary<Vector2Int, Square>();
    private Difficulty currentDifficulty;
    private float time;
    public bool playing = false;

    private List<Vector2Int> directions = new List<Vector2Int>{
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    };

    public static Difficulty[] difficulties = new[] {
        new Difficulty {
            name = "Easy",
            size = new Vector2Int(10, 10),
            bombs = 10
        },
        new Difficulty {
            name = "Medium",
            size = new Vector2Int(10, 17),
            bombs = 24
        },
        new Difficulty {
            name = "Hard",
            size = new Vector2Int(10, 17),
            bombs = 36
        },
    };

    private void Awake() {
        instance = this;
    }

    public void NewGame(Difficulty difficulty) {
        time = 0;
        firstClick = true;
        newGameButton.SetActive(false);
        highScoresButton.SetActive(false);
        var tempSquare = Instantiate(squarePrefab);
        var size = tempSquare.GetComponent<Collider2D>().bounds.size;
        Destroy(tempSquare.gameObject);

        currentDifficulty = difficulty;
        foreach (var square in squares.Values) {
            Destroy(square.gameObject);
        }
        squares.Clear();

        var bombs = new HashSet<Vector2Int>();
        while (bombs.Count < currentDifficulty.bombs) {
            bombs.Add(new Vector2Int(
                Rand.R(0, currentDifficulty.size.x),
                Rand.R(0, currentDifficulty.size.y)
            ));
        }

        var pos = new Vector2Int();
        for(pos.x = 0; pos.x < currentDifficulty.size.x; pos.x++) {
            for(pos.y = 0; pos.y < currentDifficulty.size.y; pos.y++) {
                var position = new Vector3(
                    (pos.x - (float)currentDifficulty.size.x / 2) * size.x + size.x / 2,
                    (pos.y - (float)currentDifficulty.size.y / 2) * size.y + size.y / 2,
                    0);
                var square = Instantiate(squarePrefab, position, Quaternion.identity, squaresParent);
                var adjacent = directions.Count(d => bombs.Contains(pos + d));
                square.Initialize(pos, bombs.Contains(pos), adjacent);
                squares.Add(pos, square);
            }
        }
        var camera = Camera.main;
        var canvas = Resources.FindObjectsOfTypeAll<Canvas>()[0];
        var height = (Screen.height - topBar.rect.height * canvas.scaleFactor) / Screen.height;
        camera.rect = new Rect(0, 0, 1, height);
        var widthSize = ( size.x * currentDifficulty.size.x) / (camera.aspect * 2);
        var heightSize = ( size.y * currentDifficulty.size.y) / 2;
        Debug.Log("width size: " + widthSize +
            " height size: " + heightSize);
        camera.orthographicSize = Mathf.Max(widthSize, heightSize);
        playing = true;
    }

    public void ReassignBomb() {
        while (true) {
            var pos = new Vector2Int(
                Rand.R(0, currentDifficulty.size.x),
                Rand.R(0, currentDifficulty.size.y)
            );
            var square = squares[pos];
            if (square.bomb || square.revealed) {
                continue;
            }
            square.bomb = true;
            foreach (var direction in directions) {
                if (squares.TryGetValue(pos + direction, out var other)) {
                    other.adjacent++;
                    other.Refresh();
                }
            }
            break;
        }
    }

    private void Update() {
        if (!playing) {
            return;
        }
        time += Time.deltaTime;
        clockText.text = string.Format(CultureInfo.InvariantCulture,
            "{0}:{1:00.0}", (int)time / 60, time % 60);
    }

    public void GameOver() {
        GameEnd();
        foreach (var square in squares.Values) {
            square.revealed = true;
            square.Refresh();
        }
    }

    public void CheckWin() {
        if (!Won()) {
            return;
        }
        GameEnd();
        var existingScore = PlayerPrefs.GetFloat(currentDifficulty.name + "_score", float.MaxValue);
        if (time < existingScore) {
            PlayerPrefs.SetFloat(currentDifficulty.name + "_score", time);
            DialogView.Prompt("New High Score!");
        }
        if (currentDifficulty != difficulties[0]) {
            PromptReview();
        }
    }

    private bool Won() {
        foreach (var square in squares.Values) {
            if (!square.revealed && !square.bomb) {
                return false;
            }
        }
        return true;
    }

    private void GameEnd() {
        newGameButton.SetActive(true);
        foreach (var square in squares.Values) {
            square.revealed = true;
            square.Refresh();
        }
        highScoresButton.SetActive(HighScores().Any());
        playing = false;
    }

    private bool PromptReview() {
        if (PlayerPrefs.HasKey("promptedForReview")) {
            return false;
        }
        PlayerPrefs.SetString("promptedForReview", "true");
        DialogView.Show("Are you enjoying Mine Clearer?", new List<string> { "Yes", "No" },  (option) => {
            if (option == 0) {
#if UNITY_IOS
                if (!Device.RequestStoreReview()) {
                    DialogView.Prompt("Glad to hear it!");
                }
#elif UNITY_ANDROID
                DialogView.Confirm("Would you care to write a review?", () => {
                    Application.OpenURL("market://details?id=com.styrognome.minesweeper");
                });
#else
                UIDialog.Alert("Glad to hear it!");
#endif
            }
            else {
                DialogView.Confirm("Would you like to send us feedback so we can improve?", () => {
                    Application.OpenURL("http://www.styrognome.com/contact");
                });
            }
        });
        return true;
    }

    public List<HighScore> HighScores() {
        var highScores = new List<HighScore>();
        foreach (var difficulty in difficulties) {
            var score = PlayerPrefs.GetFloat(difficulty.name + "_score", 0);
            if (score == 0) {
                continue;
            }
            highScores.Add(new HighScore{
                difficulty = difficulty,
                seconds = score
            });
        }
        return highScores;
    }

    public void RevealAdjacent(Vector2Int startPos) {
        foreach (var direction in directions) {
            var pos = startPos + direction;
            if (squares.TryGetValue(pos, out var square)) {
                square.Reveal();
            }
        }
    }
}
