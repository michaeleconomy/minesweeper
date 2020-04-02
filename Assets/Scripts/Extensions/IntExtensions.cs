using System;

public static class IntExtensions {
    public static void Times(this int times, Action<int> action) {
        for (var i = 0; i < times; i++) {
            action(i);
        }
    }

    public static string Ordinal(this int number) {
        const string TH = "th";
        string s = number.ToString();

        // Negative and zero have no ordinal representation
        if (number < 1) {
            return s;
        }

        number %= 100;
        if ((number >= 11) && (number <= 13)) {
            return s + TH;
        }

        switch (number % 10) {
            case 1: return s + "st";
            case 2: return s + "nd";
            case 3: return s + "rd";
            default: return s + TH;
        }
    }
}