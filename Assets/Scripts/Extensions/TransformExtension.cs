using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension {
    public static void DeleteChildren(this Transform transform) {
        for (var i = 0; i < transform.childCount; i++) {
            var child = transform.GetChild(i);
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static void DeleteChildrenImmediate(this Transform transform) {
        var children = new List<Transform>();
        for (var i = 0; i < transform.childCount; i++) {
            children.Add(transform.GetChild(i));
        }
        foreach (var child in children) {
            UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }
}
