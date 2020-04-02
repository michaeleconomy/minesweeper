using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Square : MonoBehaviour, IPointerClickHandler, IPointerDownHandler {
    public Vector2Int pos;
    public bool bomb, exploded, revealed, flagged;
    public int adjacent;

    public Sprite startSprite, bombSprite, explodedSprite, flagSprite;
    public Sprite[] revealedSprites;
    public SpriteRenderer sr;

    public float longPressLength;

    private bool didFlag = false;

    public void Initialize(Vector2Int pos, bool bomb, int adjacent) {
        this.bomb = bomb;
        this.adjacent = adjacent;
        this.pos = pos;
        Refresh();
    }

    private void Refresh() {
        if (!revealed) {
            sr.sprite = flagged ? flagSprite : startSprite;
            return;
        }
        if (exploded) {
            sr.sprite = explodedSprite;
            return;
        }
        if (bomb) {
            sr.sprite = bombSprite;
            return;
        }
        sr.sprite = revealedSprites[adjacent];
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        if (revealed) {
            return;
        }
        didFlag = false;
        StartCoroutine(DoFlag());
    }

    private IEnumerator DoFlag() {
        yield return new WaitForSeconds(longPressLength);
        didFlag = true;
        flagged = !flagged;
        Refresh();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        if (revealed) {
            return;
        }
        if (didFlag) {
            return;
        }
        StopAllCoroutines();

        if (flagged) {
            return;
        }
        Reveal();
    }

    public void Reveal(bool gameOver = false) {
        if (revealed) {
            return;
        }
        if (flagged) {
            return;
        }
        revealed = true;
        if (bomb) {
            if (!gameOver) {
                exploded = true;
                GameManager.instance.GameOver();
            }
            Refresh();
            return;
        }
        Refresh();
        if (adjacent == 0 && !gameOver) {
            GameManager.instance.RevealAdjacent(pos);
        }
        if (!gameOver) {
            GameManager.instance.CheckWin();
        }
    }
}