using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;

public static class ListAndArrayExtensions {

    public static string Join<T>(this IEnumerable<T> list, string seperator = ", ") {
        var sb = new StringBuilder();
        var putSeperator = false;
        foreach (var t in list) {
            if (putSeperator) {
                sb.Append(seperator);
            }
            else {
                putSeperator = true;
            }
            sb.Append(t);
        }
        return sb.ToString();
    }

    public static string ToSentence<T>(this IEnumerable<T> list) {
        if (list.Count() == 0) {
            return "";
        }

        return list.ToList().ToSentence();
    }

    public static string ToSentence<T>(this List<T> list) {
        if (list.Count == 0) {
            return "";
        }
        if (list.Count == 1) {
            return list[0].ToString();
        }

        var sb = new StringBuilder();
        sb.Append(list.GetRange(0, list.Count - 1).Join());
        if (list.Count > 2) {
            sb.Append(",");
        }
        sb.Append(" and ");
        sb.Append(list.Last().ToString());
        return sb.ToString();
    }


    public delegate bool Filter<T>(T item);

    public static T First<T>(this IEnumerable<T> enumerable, Filter<T> filter) {
        foreach (var t in enumerable) {
            if (filter(t)) {
                return t;
            }
        }
        return default(T);
    }

    public static T First<T>(this IEnumerable<T> enumerable) {
        return First(enumerable, (t) => true);
    }


    public static T Last<T>(this IEnumerable<T> enumerable, Filter<T> filter) {
        foreach (var t in enumerable.Reverse()) {
            if (filter(t)) {
                return t;
            }
        }
        return default(T);
    }

    public delegate int IntMapFunction<T>(T input);
    public delegate float FloatMapFunction<T>(T input);

    public static int Sum<T>(this IEnumerable<T> enumerable, IntMapFunction<T> map) {
        var sum = 0;
        foreach (var t in enumerable) {
            sum += map(t);
        }
        return sum;
    }

    public static float Sum<T>(this IEnumerable<T> enumerable, FloatMapFunction<T> map) {
        var sum = 0f;
        foreach (var t in enumerable) {
            sum += map(t);
        }
        return sum;
    }

    public static bool Contains<T>(this IEnumerable<T> enumerable, Filter<T> matching) where T : class {
        return enumerable.First(matching) != null;
    }


    public static bool ContainsAny<T>(this IEnumerable<T> enumerable, IEnumerable<T> ofThese) {
        foreach (var i in enumerable) {
            if (ofThese.Contains(i)) {
                return true;
            }
        }
        return false;
    }

    public static bool ContainsAny<T>(this IEnumerable<T> enumerable, params T[] ofThese) {
        foreach (var i in enumerable) {
            if (ofThese.Contains(i)) {
                return true;
            }
        }
        return false;
    }

    public static T Last<T>(this IEnumerable<T> enumerable) {
        return Last(enumerable, (t) => true);
    }

    public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable) {
        if (enumerable is HashSet<T>) {
            return (HashSet<T>)enumerable;
        }
        var set = new HashSet<T>();
        foreach (var t in enumerable) {
            set.Add(t);
        }
        return set;
    }

    public static List<T> ToList<T>(this T[] array) {
        return new List<T>(array);
    }

    public static bool Empty<T>(this IEnumerable<T> collection) {
        foreach (var i in collection) {
            return false;
        }
        return true;
    }

    public static bool Empty<T>(this ICollection<T> collection) {
        return collection.Count == 0;
    }

    public static bool Empty<T>(this Stack<T> collection) {
        return collection.Count == 0;
    }

    public static bool Empty<T>(this T[] array) {
        return array.Length == 0;
    }

    public static T Pop<T>(this List<T> list) {
        if (list.Empty()) {
            return default(T);
        }
        var t = list[0];
        list.RemoveAt(0);
        return t;
    }


    public static T PopLast<T>(this List<T> list) {
        if (list.Empty()) {
            return default(T);
        }
        var t = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return t;
    }

    public static T Pop<T>(this HashSet<T> set) {
        if (set.Empty()) {
            return default(T);
        }
        var t = set.First();
        set.Remove(t);
        return t;
    }


    public static T PopRandom<T>(this HashSet<T> set) {
        if (set.Empty()) {
            return default(T);
        }
        var t = set.RandomElement();
        set.Remove(t);
        return t;
    }

    public static T PopRandom<T>(this List<T> list) {
        if (list.Empty()) {
            return default(T);
        }
        var t = list.RandomElement();
        list.Remove(t);
        return t;
    }

    public static T[] GetRange<T>(this T[] array, int start, int count = -1) {
        if (count == -1) {
            count = array.Length - start;
        }
        var newArray = new T[count];
        for (var i = 0; i < count; i++) {
            newArray[i] = array[start + i];
        }
        return newArray;
    }

    public static List<T> Subtract<T>(this IEnumerable<T> list, IEnumerable<T> other) {
        var newList = new List<T>(list);
        newList.Remove(other);
        return newList;
    }

    public static void Remove<T>(this ICollection<T> list, IEnumerable<T> other) {
        foreach (var i in other) {
            list.Remove(i);
        }
    }

    public static List<T> GetRange<T>(this List<T> array, int start) {
        var count = array.Count - start;
        return array.GetRange(start, count);
    }

    public static T RandomElement<T>(this List<T> list) {
        if (list.Empty()) {
            return default(T);
        }
        var id = UnityEngine.Random.Range(0, list.Count);
        return list[id];
    }


    public static T RandomElement<T>(this IEnumerable<T> list) {
        return list.ToList().RandomElement();
    }


    public static T RandomElement<T>(this HashSet<T> set) {
        if (set.Empty()) {
            return default(T);
        }
        var id = UnityEngine.Random.Range(0, set.Count);
        return set.ElementAt(id);
    }


    public static T RandomElement<T>(this T[] list) {
        if (list.Empty()) {
            return default(T);
        }
        var id = UnityEngine.Random.Range(0, list.Length);
        return list[id];
    }


    public static void InsertRandomly<T>(this List<T> list, T element) {
        var id = UnityEngine.Random.Range(0, list.Count + 1);
        list.Insert(id, element);
    }

    public static List<T> Shuffle<T>(this IEnumerable<T> list) {
        var shuffled = new List<T>();
        foreach (var t in list) {
            shuffled.InsertRandomly(t);
        }
        return shuffled;
    }

    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items) {
        foreach (var item in items) {
            set.Add(item);
        }
    }

    public static void Push<T>(this List<T> list, T item) {
        list.Insert(list.Count, item);
    }

    public static bool Exists<T>(this T[] array, System.Predicate<T> match) {
        foreach (var i in array) {
            if (match(i)) {
                return true;
            }
        }
        return false;
    }

    public static int IndexOf<T>(this T[] array, T t) where T : class {
        for (var i = 0; i < array.Length; i++) {
            if (array[i] == t) {
                return i;
            }
        }
        return -1;
    }

    public static int Sum(this IEnumerable<int> enumerable) {
        var sum = 0;
        foreach (var i in enumerable) {
            sum += i;
        }
        return sum;
    }

    public static float Sum(this IEnumerable<float> enumerable) {
        var sum = 0f;
        foreach (var i in enumerable) {
            sum += i;
        }
        return sum;
    }


    public static Dictionary<TKey, List<TValue>> GroupByDict<TKey, TValue>(
            this IEnumerable<TValue> list,
            System.Func<TValue, TKey> groupByFunc) {
        var dict = new Dictionary<TKey, List<TValue>>();
        foreach (var item in list) {
            var key = groupByFunc(item);
            List<TValue> listForKey;
            if (!dict.TryGetValue(key, out listForKey)) {
                listForKey = new List<TValue>();
                dict[key] = listForKey;
            }
            listForKey.Add(item);
        }
        return dict;
    }

    public static IOrderedEnumerable<T> Sorted<T>(this IEnumerable<T> list) {
        return list.OrderBy(x => x);
    }


    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) {
        foreach (var t in list) {
            action(t);
        }
    }
}
