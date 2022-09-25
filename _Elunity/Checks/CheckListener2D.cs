using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckListener2D : MonoBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnCheck;
        public UnityEvent OnExit;
        [HideInInspector]
        public GameObject checker;

        void Update() {
            if (!TimeMgr.Instance.Paused && checker && Input.GetKeyDown(KeyCode.DownArrow)) {
                OnCheck.Invoke();
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(CheckListener2D))]
        public class Checkable2DEditor : Editor
        {
            public CheckListener2D check { get { return target as CheckListener2D; } }

            void OnEnable() {
                check.gameObject.SetLayer("Check Listener");
            }
        }
#endif
    }
}
