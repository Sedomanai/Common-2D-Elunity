using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Elang
{
    [System.Serializable]
    public class Camera2DFlagsX
    {
        public bool freeze;
        public bool snap;
        public float smoothing = 1.0f;
        public float snapPoint;

        public static Camera2DFlagsX Empty() {
            var newX = new Camera2DFlagsX();
            newX.freeze = false;
            newX.snap = false;
            newX.smoothing = 0.0f;
            return newX;
        }

#if UNITY_EDITOR
        public void OnInspector(bool hasSnapPoint) {
            EditorGUI.indentLevel++;
            freeze = EditorGUILayout.Toggle("Freeze", freeze);
            if (freeze) {
                EditorGUI.indentLevel++;
                snap = EditorGUILayout.Toggle("Snap", snap);
                if (hasSnapPoint)
                    snapPoint = EditorGUILayout.FloatField("Snap Point", snapPoint);
                EditorGUI.indentLevel--;
            }
            smoothing = EditorGUILayout.FloatField("Smoothing", smoothing);
            EditorGUI.indentLevel--;
        }
#endif
    }

    [System.Serializable]
    public class Camera2DFlagsY : Camera2DFlagsX
    {
        public float ceiling;
        public float floor;
        public new static Camera2DFlagsY Empty() {
            var newY = new Camera2DFlagsY();
            newY.freeze = false;
            newY.snap = false;
            newY.smoothing = 0.0f;
            newY.ceiling = 0.0f;
            newY.floor = 0.0f;
            return newY;
        }

#if UNITY_EDITOR
        public void OnInspector2() {
            EditorGUI.indentLevel++;
            ceiling = EditorGUILayout.FloatField("Ceiling", ceiling);
            floor = EditorGUILayout.FloatField("Floor", floor);
            EditorGUI.indentLevel--;
        }
#endif
    }
}

