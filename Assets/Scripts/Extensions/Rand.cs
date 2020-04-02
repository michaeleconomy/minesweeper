using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Rand {

    public static float R() {
        return R(0f, 1f);
    }

    public static float R(float max) {
        return R(0f, max);
    }

    public static float R(float min, float max) {
        return UnityEngine.Random.Range(min, max);
    }

    public static int R(int max) {
        return R(0, max);
    }

    public static int R(int min, int max) {
        return UnityEngine.Random.Range(min, max);
    }


    public static bool P(float percentage) {
        return R() < percentage;
    }

    public static bool B() {
        return UnityEngine.Random.Range(0, 2) == 1;
    }

    public static Vector2 RandomPoint(this Rect rect) {
        return new Vector2(R(rect.xMin, rect.xMax), R(rect.yMin, rect.yMax));
    }

    private static string vowels = "aeiouy";
    private static string consonants = "bcdfghjklmnpqrstvwyz";
    private static string terminator = ".!?";
    private static bool[][] syllables = new[] { new[] {true }, new[] { true, false }, new[] { false, true }, new[] { false, true, false }, new[] { false, false, true }, new[] { true, false } };

    public static string RandomLetter(this string s) {
        return s.Substring(R(s.Length), 1);
    }

    public static string Syllable() {
        return syllables.RandomElement().Select(vowel => {
            if (vowel) {
                return vowels.RandomLetter();
            }
            return consonants.RandomLetter();
        }).Join("");
    }

    public static string Word() {
        var s = "";
        R(1, 8).Times(i => {
            s += Syllable();
        });
        return s;
    }

    public static string Name() {
        return Word() + " " + Word();
    }

    public static string Sentence() {
        var s = new List<string>();
        R(2, 10).Times(i => {
            s.Add(Word());
        });
        s[0] = s[0].Substring(0, 1).ToUpper() + s[0].Substring(1);
        return s.Join(" ") + terminator.RandomLetter() + " ";
    }
}
